using System.IO;
using UnityEngine;

namespace Caveman.Network
{
    public class ServerMessage
    {
        public ushort Length { get; set; }
        public string Content { get; set; }

        public static ServerMessage MessageFromStream(StreamReader reader)
        {
            string buffer;

            buffer = reader.ReadToEnd();
            Debug.Log(buffer);

            return new ServerMessage(buffer);
        }

        private ServerMessage(string data)
        {
            Content = data;
        }
    }

}