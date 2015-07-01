using System;
using System.IO;
using UnityEngine;

namespace Caveman.Network
{
    public class ServerMessage
    {
        private const string MESSAGE_MARKER = "#";

        private readonly JSONObject contentObject;

        public static ServerMessage[] MessageListFromStream(StreamReader reader)
        {
            var buffer = reader.ReadToEnd();
            Debug.Log(buffer);


            if (String.IsNullOrEmpty(buffer))
                return null;


            string[] bufferChanks = buffer.Split((MESSAGE_MARKER + MESSAGE_MARKER).ToCharArray());

            var result = new ServerMessage[bufferChanks.Length];

            for (int i = 0; i < bufferChanks.Length; ++i)
            {
                string chank = bufferChanks[i];
                chank = chank.Trim(MESSAGE_MARKER.ToCharArray());

                try{
                    var json = new JSONObject(chank);
                    result[i] = new ServerMessage(json);
                } catch (Exception e) {
                    Debug.Log(e.ToString());
                    result[i] = null;
                    break;
                }
            }

            return result;
        }

        private ServerMessage(JSONObject data)
        {
            contentObject = data;
        }

        public void SendMessageToListener(IServerListener listener)
        {
            string actionType = contentObject.GetField(ServerParams.ACTION_TYPE).ToString();
            SendMessageToListener(listener, actionType);
        }

        public void SendMessageToListener(IServerListener listener, string actionType)
        {
            if (actionType.Equals(ServerParams.STONE_ADDED_ACTION))
            {
                float x = contentObject[ServerParams.X].f;
                float y = contentObject[ServerParams.Y].f;
                listener.StoneAdded(x, y);
            }
            else if (actionType.Equals(ServerParams.STONE_REMOVED_ACTION))
            {
                float x = contentObject[ServerParams.X].f;
                float y = contentObject[ServerParams.Y].f;
                listener.StoneRemoved(x, y);
            }
            else if (actionType.Equals(ServerParams.MOVE_ACTION))
            {
                var player = contentObject[ServerParams.PLAYER].str;
                var x = contentObject[ServerParams.X].f;
                var y = contentObject[ServerParams.Y].f;
                listener.Move(player, x, y);
            }
        }
    }

}