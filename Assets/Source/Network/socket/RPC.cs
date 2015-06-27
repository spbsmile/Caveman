using System;
using System.Net;
using System.Text;

using System.IO;
using UnityEngine;
using System.Net.Sockets;
using System.Threading;
using System.Collections.Generic;

namespace Caveman.Network
{
    public enum ActionType: string
    {
        Login = "login",
        UseWeapon = "use_weapon",
        PickWeapon = "pick_weapon",
        PickBonus = "pick_bonus"
    }


    public class RPC
    {
        private const string IP = "188.166.37.212";
        private const int PORT = 8080;

        private TcpClient client;
        private StreamReader reader;
        private StreamWriter writer;
        private Thread networkThread;
        private readonly Queue<ServerMessage> messageQueue = new Queue<ServerMessage>();
        private Boolean socketReady = false;

        private RPC()
        {
        }

        static private RPC instance;
        static RPC Instance
        {
            get
            {
                if (instance == null)
                    instance = new RPC();
                return instance;
            }
        }

        // API

        void Update()
        {

        }

        void StartSession()
        {
             if (client == null)
               {
                 string server = IP;
                 int port = PORT;
                 client = new TcpClient(server, port);
                 Stream stream = client.GetStream();
                 reader = new StreamReader(stream);
                 writer = new StreamWriter(stream);
             }

             ClientMessage msg = ClientMessage.MessageWithActionType(ActionType.Login);
             writeString(msg.Content);
        }

        void UseWeapon(Vector2 point, int weaponType)
        {
             ClientMessage msg = ClientMessage.MessageWithActionType(ActionType.UseWeapon);
             writeString(msg.Content);
        }

        void PickWeapon(Vector2 point, int weaponType)
        {
             ClientMessage msg = ClientMessage.MessageWithActionType(ActionType.PickWeapon);
             writeString(msg.Content);
        }

        void PickBonus(Vector2 point, int bonusType)
        {
             ClientMessage msg = ClientMessage.MessageWithActionType(ActionType.PickBonus);
             writeString(msg.Content);
        }

        // private functions


        private void writeString(string str)
        {
             writer.Write(str);
             writer.Flush();
        }

        //NOTE: OLD CODE

        static void addItemToQueue(Message item)
        {
            lock(messageQueue)
            {
                messageQueue.Enqueue(item);
            }
        }

        static Message getItemFromQueue()
        {
            lock(messageQueue)
            {
                if (messageQueue.Count > 0)
                {
                    return messageQueue.Dequeue();
                }
                else
                {
                    return null;
                }
            }
        }

        static void processMessage()
        {
            Message msg = getItemFromQueue();
            if (msg != null)
            {
                Debug.Log (msg.content);
                // do some processing here, like update the player state
            }
        }

        void startServer()
        {
            connect();
        }

    //  static void startServer()
    //    {
    //      if (networkThread == null)
    //        {
    //          connect();
    //          networkThread = new Thread(() =>
    //            {
    //              while (reader != null)
    //                {
    //                  Message msg = Message.ReadFromStream(reader);
    //                  addItemToQueue(msg);
    //              }
    //              lock(networkThread)
    //                {
    //                  networkThread = null;
    //              }
    //          });
    //          networkThread.Start();
    //      }
    //  }

        void connect()
        {
            byte[] bytes = new byte[1024];

            // Connect to a remote device.
            // Establish the remote endpoint for the socket.
            // This example uses port 11000 on the local computer.
            IPHostEntry ipHostInfo = Dns.Resolve(ConnectionIp);
            IPAddress ipAddress = ipHostInfo.AddressList[0];
            IPEndPoint remoteEP = new IPEndPoint(ipAddress,ConnectionPort);

            // Create a TCP/IP  socket.
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp );

            try {
                socket.Connect(remoteEP);

                print("Socket connected to : " +
                    socket.RemoteEndPoint.ToString());

                // Encode the data string into a byte array.

                var json = new JSONObject(JSONObject.Type.OBJECT);
                json.AddField("action","login");
                json.AddField("id",1);
                json.AddField("name","petya");
                var messageStr = "#" + json + "#";
                byte[] msg = Encoding.ASCII.GetBytes(messageStr);
                //Debug.Log ("WTF " + str);

                // Send the data through the socket.
                int bytesSent = socket.Send(msg);

                // Receive the response from the remote device.
                int bytesRec = socket.Receive(bytes);
                print("Echoed test = " +
                    Encoding.ASCII.GetString(bytes,0,bytesRec));

                // Release the socket.
    //          socket.Shutdown(SocketShutdown.Both);
    //          socket.Close();

            } catch (ArgumentNullException ane) {
                print("ArgumentNullException : " +ane);
            } catch (SocketException se) {
                print("SocketException : " +se);
            } catch (Exception e) {
                print("Unexpected exception : " + e);
            }
        }

    //  static void connect()
    //    {
    //      if (client == null)
    //        {
    //          string server = ConnectionIp;
    //          int port = ConnectionPort;
    //          client = new TcpClient(server, port);
    //          Stream stream = client.GetStream();
    //          reader = new StreamReader(stream);
    //          writer = new StreamWriter(stream);
    //      }
    //  }

        public static void send(Message msg)
        {
            msg.WriteToStream(writer);
            writer.Flush();
        }


        public void setupSocket() {
            try {
                client = new TcpClient();
                client.Connect(new IPEndPoint(IPAddress.Parse(connectionIP), connectionPort));
                ns = client.GetStream();
                ns.ReadTimeout = 1;
                theWriter = new BinaryWriter(ns);
                theReader = new BinaryWriter(ns);
                socketReady = true;

            }
            catch (Exception e) {
                Debug.Log("Socket error: " + e);
            }
        }

        public void writeSocket(string theLine) {
            if (!socketReady)
                return;
            String foo = "#" + theLine + "#";

            theWriter.Write(foo);
            theWriter.Flush();
        }

        public String readSocket() {
            if (!socketReady)
                return "";
            try {
                string wtf = theReader.ReadLine();
                Debug.Log("MSG " + wtf);
                return wtf;
            } catch (Exception e) {
                Debug.Log("ERR " + e.ToString());
                return "";
            }
        }

    }
}


