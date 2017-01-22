using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Caveman.Setting;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace Caveman.Network
{
    public class ServerConnection
    {
        //"127.0.0.1/185.117.155.113/10.0.1.17 "; 
        private const int Port = 8080;

        private readonly Queue<ServerMessageManager> messageQueue = new Queue<ServerMessageManager>();

        private TcpClient client;
        private float lastTimeUpdated;
        private Thread networkThread;
        private StreamReader reader;
        private StreamWriter writer;
        private string clientId;

        protected ServerConnection()
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
            if (Time.timeSinceLevelLoad - lastTimeUpdated > Settings.ServerPingTime)
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

        public void StartSession(string userId, string userName, bool isObservableMode)
        {
            clientId = userId;
            if (client == null)
            {
                try
                {
                    var ipServer = PlayerPrefs.HasKey(Settings.KeyIpServer)
                        ? PlayerPrefs.GetString(Settings.KeyIpServer)
                        : Settings.IP_SERVER;
                    client = new TcpClient(ipServer, Port);
                    var stream = client.GetStream();

                    reader = new StreamReader(stream, Encoding.UTF8);
                    writer = new StreamWriter(stream);

                    if (!isObservableMode)
                    {
                        SendLogin(userName);
                    }
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

        private void SendTick()
        {
            SendMessageToSocket(new JObject
            {
                {ServerParams.ActionType, ServerParams.PingAction}
            });
        }

        private void SendLogin(string userName)
        {
            SendMessageToSocket(new JObject
            {
                {ServerParams.ActionType, ServerParams.LoginAction},
                {ServerParams.UserName, userName}
            });
        }

        private void SendStringToSocket(string str)
        {
            if (writer != null)
            {
                writer.Write(str);
                writer.Flush();
            }
        }

        protected void SendMessageToSocket(JObject jObject)
        {
            if (jObject == null) return;
            jObject.Add(ServerParams.UserId, clientId);
            SendStringToSocket(ParseContentForServer(jObject));
        }

        private string ParseContentForServer(JObject jObject)
        {
            return jObject != null ? "#" + jObject + "#" : "";
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
                        AddItemToQueue(new ServerMessageManager(result));
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

        private void AddItemToQueue(ServerMessageManager item)
        {
            lock (messageQueue)
            {
                messageQueue.Enqueue(item);
            }
        }

        private ServerMessageManager GetItemFromQueue()
        {
            lock (messageQueue)
            {
                return messageQueue.Count > 0 ? messageQueue.Dequeue() : null;
            }
        }
    }
}