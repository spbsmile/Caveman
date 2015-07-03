using System;
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

        private RPC()
        {
            lastTimeUpdated = Time.timeSinceLevelLoad;
        }

        public IServerListener ServerListener { get; set; }

        private static RPC Instance
        {
            get
            {
                if (instance == null)
                    instance = new RPC();
                return instance;
            }
        }

        // API

        /**
            Sends tick if it is time
            Checks if there are messages for client and sends them via listener interface
        */

        private void Update()
        {
            if (Time.timeSinceLevelLoad - lastTimeUpdated > 0.2)
            {
                lastTimeUpdated = Time.timeSinceLevelLoad;
                SendTick();
            }

            if (ServerListener != null)
            {
                ServerMessage message = GetItemFromQueue();
                while (message != null)
                {
                    message.SendMessageToListener(ServerListener);
                    message = GetItemFromQueue();
                }
            }
        }

        private void SendTick()
        {
            ClientMessage msg = ClientMessage.TickMessage();
            SendStringToSocket(msg.Content);
        }

        /**
         * Runs session and starts listen to the server
         * */

        private void StartSession()
        {
            if (client == null)
            {
                try
                {
                    client = new TcpClient(IP, PORT);
                    Stream stream = client.GetStream();
                    //                 ns.ReadTimeout = 1;
                    reader = new StreamReader(stream);
                    writer = new StreamWriter(stream);

                    StartListeningServer();

                    var actionParams = new Dictionary<string, string>();
                    actionParams.Add(ServerParams.USER_NAME, "Petya");
                    ClientMessage msg = ClientMessage.LoginMessage(actionParams);
                    SendStringToSocket(msg.Content);
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

        private void stopSession()
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

        private void SendUseWeapon(Vector2 point, int weaponType)
        {
            ClientMessage msg = ClientMessage.UseWeapon(point.x, point.y);
            if (msg != null)
                SendStringToSocket(msg.Content);
        }

        private void SendPickWeapon(Vector2 point, int weaponType)
        {
            ClientMessage msg = ClientMessage.PickWeapon(point.x, point.y);
            if (msg != null)
                SendStringToSocket(msg.Content);
        }

        private void SendPickBonus(Vector2 point, int bonusType)
        {
            ClientMessage msg = ClientMessage.PickBonus(point.x, point.y);
            if (msg != null)
                SendStringToSocket(msg.Content);
        }

        // private functions


        private void SendStringToSocket(string str)
        {
            writer.Write(str);
            writer.Flush();
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
                    try
                    {
                        ServerMessage[] msgs = ServerMessage.MessageListFromStream(reader);
                        foreach (ServerMessage msg in msgs)
                        {
                            AddItemToQueue(msg);
                        }
                    }
                    catch (Exception e)
                    {
                        Debug.Log("ERR " + e);
                        break;
                    }
                }

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
    }
}