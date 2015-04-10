using System;
using System.IO;
using UnityEngine;
using AssemblyCSharp;

public class Message {
	public ushort length { get; set; }
	public string content { get; set; }
	
	public static Message ReadFromStream(StreamReader reader) {
		//ushort len;
		//byte[] len_buf;
		string buffer;
		
		/*len_buf = reader.ReadBytes(2);
		if (BitConverter.IsLittleEndian) {
			Array.Reverse(len_buf);
		}
		len = BitConverter.ToUInt16(len_buf, 0);
		
		buffer = reader.ReadBytes(len);*/

		buffer = reader.ReadToEnd();
		Debug.Log (buffer);
		
		return new Message(buffer);
	}
	
	public void WriteToStream(StreamWriter writer) {
		byte[] len_bytes = BitConverter.GetBytes(length);
		
		if (BitConverter.IsLittleEndian) {
			Array.Reverse(len_bytes);
		}
		//writer.Write(len_bytes);
		//GetString (content);
		Debug.Log ("SEND " + content);
		writer.Write(content);
	}

	public static byte[] GetBytes(string str)
	{
		//string jsonStr = Encoding.UTF8.GetString(str);
		//return JsonConvert.DeserializeObject<Dictionary<String, Object>>(jsonStr);

		byte[] bytes = new byte[str.Length * sizeof(char)];
		System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);

		return bytes;
	}
	
	public static string GetString(byte[] bytes)
	{
		char[] chars = new char[bytes.Length / sizeof(char)];
		System.Buffer.BlockCopy(bytes, 0, chars, 0, bytes.Length);
		Debug.Log ("EPTA " + new string (chars));
		return new string(chars);
	}
	
	public Message(string data) {
		content = data;
	}
}