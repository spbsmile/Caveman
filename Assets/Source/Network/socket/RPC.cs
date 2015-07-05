using System;
using System.Text;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using UnityEngine;

namespace Caveman.Network
{
    public interface IServerListener
    {
        void StoneAddedReceived(Vector2 point);
        void StoneRemovedReceived(Vector2 point);
        void MoveReceived(string player, Vector2 point);
        void LoginReceived(string player);
        void PickWeaponReceived(string player, Vector2 point);
        void PickBonusReceived(string player, Vector2 point);
        void UseWeaponReceived(string player, Vector2 point);
    }


    public class RPC
    {
        private const float SERVER_PINT_TIME = 0.2f;

        private const string IP = "188.166.37.212";
        private const int PORT = 8080;
        private static RPC instance;
        private readonly Queue<ServerMessage> messageQueue = new Queue<ServerMessage>();

        private TcpClient client;
        private float lastTimeUpdated;
        private Thread networkThread;
        private StreamReader reader;
        private StreamWriter writer;

        private string clientId;

        public RPC()
        {
            lastTimeUpdated = Time.timeSinceLevelLoad;
        }

        public IServerListener ServerListener { get; set; }

        // API

        /**
            Sends tick if it is time
            Checks if there are messages for client and sends them via listener interface
        */

        public void Update()
        {
            if (Time.timeSinceLevelLoad - lastTimeUpdated > SERVER_PINT_TIME)
            {
                lastTimeUpdated = Time.timeSinceLevelLoad;
                SendTick();
            }

            if (ServerListener != null)
            {
                var message = GetItemFromQueue();
                while (message != null)
                {
                    message.SendMessageToListener(ServerListener);
                    message = GetItemFromQueue();
                }
            }
        }


        /**
         * Runs session and starts listen to the server
         * */

        public void StartSession(string userId, string userName)
        {
            clientId = userId;
            if (client == null)
            {
                try
                {
                    client = new TcpClient(IP, PORT);
                    var stream = client.GetStream();
                    //stream.ReadTimeout = 1;
                    reader = new StreamReader(stream, Encoding.UTF8);
                    writer = new StreamWriter(stream);

                    SendLogin(userName);
                    StartListeningServer();

                }
                catch (Exception e)
                {
                    Debug.Log("Socket error: " + e);
                }
            }
        }

        /**
         * Stops opened session
         */

        public void StopSession()
        {
            if (client != null)
            {
                client.Close();
                client = null;
                reader.Close();
                reader = null;
                writer.Close();
                writer = null;
            }
        }

        public void SendUseWeapon(Vector2 point, int weaponType)
        {
            SendMessageToSocket(ClientMessage.UseWeapon(point.x, point.y));
        }

        public void SendPickWeapon(Vector2 point, int weaponType)
        {
            SendMessageToSocket(ClientMessage.PickWeapon(point.x, point.y));
        }

        public void SendPickBonus(Vector2 point, int bonusType)
        {
            SendMessageToSocket(ClientMessage.PickBonus(point.x, point.y));
        }


        // private methods

        private void SendTick()
        {
            SendMessageToSocket(ClientMessage.TickMessage());
        }

        private void SendLogin(string userName)
        {
            SendMessageToSocket(ClientMessage.LoginMessage(userName));
        }

        private void SendStringToSocket(string str)
        {
            if (writer != null)
            {
                writer.Write(str);
                writer.Flush();
            }
        }

        private void SendMessageToSocket(ClientMessage msg)
        {
            if (msg != null)
            {
                CompleteClientMessage(msg);
                SendStringToSocket(msg.Content);
            }
        }

        /**
            Listens to the server while Reader is not null
        */

        private void StartListeningServer()
        {
            networkThread = new Thread(() =>
            {
                while (reader != null)
                {
                    try{
                        char[] chars = new char[1024];

                        string result = "";
                        char currentChar;

                        while ((currentChar = (char)reader.Read()) != '&')
                        {
                            if (currentChar != '#')
                                result += currentChar;
                        }

                        ServerMessage msg = new ServerMessage(result);
                        AddItemToQueue(msg);
                    } catch (Exception e)
                    {
                        Debug.Log("socket read error : " + e);
                        break;
                    }
                }

                Debug.Log("finishing listening socket");

                lock (networkThread)
                {
                    networkThread = null;
                }
            });
            networkThread.Start();
        }

        private void AddItemToQueue(ServerMessage item)
        {
            lock (messageQueue)
            {
                messageQueue.Enqueue(item);
            }
        }

        private ServerMessage GetItemFromQueue()
        {
            lock (messageQueue)
            {
                return messageQueue.Count > 0 ? messageQueue.Dequeue() : null;
            }
        }

        private void CompleteClientMessage(ClientMessage msg)
        {
            msg.addParam(ServerParams.USER_ID, clientId);
        }
    }
}