using System;
using System.IO;
using UnityEngine;
using AssemblyCSharp;
using System.Net.Sockets;
using System.Net;
using System.Runtime.Serialization;
using System.Collections;
using System.Threading;
using System.Collections.Generic;

public class GameControl : MonoBehaviour
{
	void Awake() {
		//DontDestroyOnLoad(this);
	}
	
	// Use this for initialization
	void Start() {/*
		startServer();

		JSONObject json = new JSONObject(JSONObject.Type.OBJECT);

		json.AddField("action","login");
		json.AddField("id",1);
		json.AddField("name","petya");
		string str = "#" + json.ToString() + "#";
		Message msg = new Message (str);
		Debug.Log ("WTF " + str);
		send (msg);*/
	}
	
	// Update is called once per frame
	void Update() {
		//processMessage();
	}



	static string connectionIP = "198.211.120.236";
	static int connectionPort = 8080;

	static TcpClient client = null;
	static StreamReader reader = null;
	static StreamWriter writer = null;
	static Thread networkThread = null;
	private static Queue<Message> messageQueue = new Queue<Message>();

	internal Boolean socketReady = false;

	static void addItemToQueue(Message item) {
		lock(messageQueue) {
			messageQueue.Enqueue(item);
		}
	}
	
	static Message getItemFromQueue() {
		lock(messageQueue) {
			if (messageQueue.Count > 0) {
				return messageQueue.Dequeue();
			} else {
				return null;
			}
		}
	}
	
	static void processMessage() {
		Message msg = getItemFromQueue();
		if (msg != null) {
			Debug.Log (msg.content);
			// do some processing here, like update the player state
		}
	}
	
	static void startServer() {
		if (networkThread == null) {
			connect();
			networkThread = new Thread(() => {
				while (reader != null) {
					Message msg = Message.ReadFromStream(reader);
					addItemToQueue(msg);
				}
				lock(networkThread) {
					networkThread = null;
				}
			});
			networkThread.Start();
		}
	}
	
	static void connect() {
		if (client == null) {
			string server = connectionIP;
			int port = connectionPort;
			client = new TcpClient(server, port);
			Stream stream = client.GetStream();
			reader = new StreamReader(stream);
			writer = new StreamWriter(stream);
		}
	}
	
	public static void send(Message msg) {
		msg.WriteToStream(writer);
		writer.Flush();
	}


	/*
	void Start ()
	{
		if (Settings.GAME_TYPE == 1)
		{
			setupSocket();

			JSONObject json = new JSONObject(JSONObject.Type.OBJECT);
			json.AddField("action","login");
			json.AddField("id",1);
			json.AddField("name","petya");

			writeSocket(json.ToString());
			readSocket();
		}
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
	
	void Update ()
	{

	}*/
}

