using System;
using System.Text;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using Caveman.Setting;
using UnityEngine;

namespace Caveman.Network
{
    public interface IServerListener
    {
        void WeaponAddedReceived(string key, Vector2 point);
        void WeaponRemovedReceived(string key);
        void MoveReceived(string playerId, Vector2 point);
        void LoginReceived(string playerId);
        void PickWeaponReceived(string playerId, string key);
        void PickBonusReceived(string playerId, string key);
        void UseWeaponReceived(string playerId, Vector2 aim);
        void RespawnReceived(string playerId, Vector2 point);
        void BonusAddedReceived(string key, Vector2 point);
        void PlayerDeadResceived(string playerId);
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
            var serverPoint = Convector(point);
            SendMessageToSocket(ClientMessage.UseWeapon(serverPoint.x, serverPoint.y));
        }

        public void SendPickWeapon(Vector2 point, int weaponType)
        {
            var serverPoint = Convector(point);
            SendMessageToSocket(ClientMessage.PickWeapon(serverPoint.x, serverPoint.y));
        }

        public void SendPickBonus(Vector2 point, int bonusType)
        {
            var serverPoint = Convector(point);
            SendMessageToSocket(ClientMessage.PickBonus(serverPoint.x, serverPoint.y));
        }

        public void SendRespawn(Vector2 point)
        {
            var serverPoint = Convector(point);
            SendMessageToSocket(ClientMessage.Respawn(serverPoint.x, serverPoint.y));
        }

        public void SendPlayerDead()
        {
            SendMessageToSocket(ClientMessage.PlayerDead());
        }

        public void SendMove(Vector2 point)
        {
            var serverPoint = Convector(point);
            SendMessageToSocket(ClientMessage.Move(serverPoint.x, serverPoint.y));
        }

        private Vector2 Convector(Vector2 point)
        {
            var x = (point.x / Settings.HeightMap) * Multiplayer.HeigthMapServer;
            var y = (point.y / Settings.WidthMap) * Multiplayer.WidthMapServer;
            return new Vector2(x, y);
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