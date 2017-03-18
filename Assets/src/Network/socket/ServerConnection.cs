using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Caveman.DevSetting;
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

        public void Update()
        {
            if (Time.timeSinceLevelLoad - lastTimeUpdated > DevSettings.ServerPingTime)
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

        public void StartSession(string userId, string userName, bool isObservableMode)
        {
            clientId = userId;
            if (client == null)
            {
                try
                {
                    var ipServer = PlayerPrefs.HasKey(DevSettings.KeyIpServer)
                        ? PlayerPrefs.GetString(DevSettings.KeyIpServer)
                        : DevSettings.IP_SERVER;
                    client = new TcpClient(ipServer, Port);
                    var stream = client.GetStream();

                    reader = new StreamReader(stream, Encoding.UTF8);
                    writer = new StreamWriter(stream);

                    StartListeningServer();                   
                    SendLogin(userName, isObservableMode);                
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

        private void SendLogin(string userName, bool isObservable)
        {
            SendMessageToSocket(new JObject
            {
                {ServerParams.ActionType, ServerParams.LoginAction},
                {ServerParams.UserName, userName},
                {ServerParams.IsObservable, isObservable}
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