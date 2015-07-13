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
        void WeaponAddedReceived(Vector2 point);
        void WeaponRemovedReceived(Vector2 point);
        void MoveReceived(string playerId, Vector2 point);
        void LoginReceived(string playerId);
        void PickWeaponReceived(string playerId, Vector2 point);
        void PickBonusReceived(string playerId, Vector2 point);
        void UseWeaponReceived(string playerId, Vector2 point);
        void RespawnReceived(string playerId, Vector2 point);
        void BonusAddedReceived(Vector2 point);
        void PlayerDeadResceived(string playerId, Vector2 point);
    }

    public class ServerConnection
    {
        private const float ServerPingTime = 0.2f;
        private const string Ip = "188.166.37.212";
        private const int Port = 8080;

        private static ServerConnection instance;
        private readonly Queue<ServerMessage> messageQueue = new Queue<ServerMessage>();

        private TcpClient client;
        private float lastTimeUpdated;
        private Thread networkThread;
        private StreamReader reader;
        private StreamWriter writer;
        private string clientId;

        public ServerConnection()
        {
            lastTimeUpdated = Time.timeSinceLevelLoad;
        }

        public IServerListener ServerListener { private get; set; }

        // API

        /**
            Sends tick if it is time
            Checks if there are messages for client and sends them via listener interface
        */

        public void Update()
        {
            if (Time.timeSinceLevelLoad - lastTimeUpdated > ServerPingTime)
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
                    client = new TcpClient(Ip, Port);
                    var stream = client.GetStream();

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

        public void SendRespawn(string playerId)
        {
            SendMessageToSocket(ClientMessage.Respawn(playerId));
        }

        public void SendPlayerDead(string playerId)
        {
            SendMessageToSocket(ClientMessage.PlayerDead(playerId));
        }

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
                    try
                    {
                        var result = "";
                        char currentChar;

                        while ((currentChar = (char)reader.Read()) != '&')
                        {
                            if (currentChar != '#')
                                result += currentChar;
                        }
                        AddItemToQueue(new ServerMessage(result));
                    }
                    catch (Exception e)
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
            msg.AddParam(ServerParams.UserId, clientId);
        }
    }
}