using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

using System.IO;
using UnityEngine;
using System.Net.Sockets;
using System.Threading;
using System.Collections;
using System.Collections.Generic;

public class RPC : MonoBehaviour
{
	void Awake() 
    {
		//DontDestroyOnLoad(this);
	}
	
	// Use this for initialization
	void Start()
    {
        startServer();

//        var json = new JSONObject(JSONObject.Type.OBJECT);
//
//        json.AddField("action","login");
//        json.AddField("id",1);
//        json.AddField("name","petya");
//        var str = "#" + json + "#";
//        var msg = new Message (str);
//        Debug.Log ("WTF " + str);
//        send (msg);
	}
	
	// Update is called once per frame
	void Update() 
    {
		processMessage();
	}


	private const string ConnectionIp = "188.166.37.212";
    private const int ConnectionPort = 8080;

	private Socket socket; 

    static TcpClient client;
	static StreamReader reader;
	static StreamWriter writer;
	static Thread networkThread;
	private static readonly Queue<Message> messageQueue = new Queue<Message>();

	internal Boolean socketReady = false;

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
	
//	static void startServer() 
//    {
//		if (networkThread == null) 
//        {
//			connect();
//			networkThread = new Thread(() => 
//            {
//				while (reader != null) 
//                {
//					Message msg = Message.ReadFromStream(reader);
//					addItemToQueue(msg);
//				}
//				lock(networkThread)
//                {
//					networkThread = null;
//				}
//			});
//			networkThread.Start();
//		}
//	}

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
//			socket.Shutdown(SocketShutdown.Both);
//			socket.Close();

		} catch (ArgumentNullException ane) {
			print("ArgumentNullException : " +ane.ToString());
		} catch (SocketException se) {
			print("SocketException : " +se.ToString());
		} catch (Exception e) {
			print("Unexpected exception : " + e.ToString());
		}
	}

//	static void connect() 
//    {
//		if (client == null) 
//        {
//			string server = ConnectionIp;
//			int port = ConnectionPort;
//			client = new TcpClient(server, port);
//			Stream stream = client.GetStream();
//			reader = new StreamReader(stream);
//			writer = new StreamWriter(stream);
//		}
//	}
	
	public static void send(Message msg) 
    {
		msg.WriteToStream(writer);
		writer.Flush();
	}


//	void Start ()
//	{
//		if (Settings.GAME_TYPE == 1)
//		{
//			setupSocket();
//
//			JSONObject json = new JSONObject(JSONObject.Type.OBJECT);
//			json.AddField("action","login");
//			json.AddField("id",1);
//			json.AddField("name","petya");
//
//			writeSocket(json.ToString());
//			readSocket();
//		}
//	}
//
//	public void setupSocket() { 
//		try {
//			client = new TcpClient();
//			client.Connect(new IPEndPoint(IPAddress.Parse(connectionIP), connectionPort));
//			ns = client.GetStream(); 
//			ns.ReadTimeout = 1;
//			theWriter = new BinaryWriter(ns);
//			theReader = new BinaryWriter(ns);
//			socketReady = true;
//
//		}
//		catch (Exception e) {
//			Debug.Log("Socket error: " + e);
//		}
//	}
//
//	public void writeSocket(string theLine) {
//		if (!socketReady)
//			return;
//		String foo = "#" + theLine + "#";
//
//		theWriter.Write(foo);
//		theWriter.Flush();
//	}
//
//	public String readSocket() {
//		if (!socketReady)
//			return "";
//		try {
//			string wtf = theReader.ReadLine();
//			Debug.Log("MSG " + wtf);
//			return wtf;
//		} catch (Exception e) {
//			Debug.Log("ERR " + e.ToString());
//			return "";
//		}
//	}
//	
//	void Update ()
//	{
//
//	}
}

