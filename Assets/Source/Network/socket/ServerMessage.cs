using System;
using System.IO;
using UnityEngine;

namespace Caveman.Network
{
    public class ServerMessage
    {
        private const string MESSAGE_MARKER = "#";
        private const string MESSAGE_DELIMITER = "#&";

        private readonly JSONObject contentObject;

        private ServerMessage(JSONObject data)
        {
            contentObject = data;
        }

        public static ServerMessage[] MessageListFromStream(StreamReader reader)
        {
            string buffer = reader.ReadToEnd();
            Debug.Log(buffer);


            if (String.IsNullOrEmpty(buffer))
                return null;


            string[] bufferChanks = buffer.Split((MESSAGE_DELIMITER).ToCharArray());

            var result = new ServerMessage[bufferChanks.Length];

            for (int i = 0; i < bufferChanks.Length; ++i)
            {
                string chank = bufferChanks[i];
                chank = chank.Trim(MESSAGE_MARKER.ToCharArray());

                try
                {
                    var json = new JSONObject(chank);
                    result[i] = new ServerMessage(json);
                }
                catch (Exception e)
                {
                    Debug.Log(e.ToString());
                    result[i] = null;
                    break;
                }
            }

            return result;
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
                listener.StoneAddedReceived(new Vector2(x, y));
            }
            else if (actionType.Equals(ServerParams.STONE_REMOVED_ACTION))
            {
                float x = contentObject[ServerParams.X].f;
                float y = contentObject[ServerParams.Y].f;
                listener.StoneRemovedReceived(new Vector2(x, y));
            }
            else if (actionType.Equals(ServerParams.MOVE_ACTION))
            {
                string player = contentObject[ServerParams.PLAYER].str;
                float x = contentObject[ServerParams.X].f;
                float y = contentObject[ServerParams.Y].f;
                listener.MoveReceived(player, new Vector2(x, y));
            }
            else if (actionType.Equals(ServerParams.PICK_BONUS_ACTION))
            {
                string player = contentObject[ServerParams.PLAYER].str;
                float x = contentObject[ServerParams.X].f;
                float y = contentObject[ServerParams.Y].f;
                listener.PickBonusReceived(player, new Vector2(x, y));
            }
            else if (actionType.Equals(ServerParams.PICK_WEAPON_ACTION))
            {
                string player = contentObject[ServerParams.PLAYER].str;
                float x = contentObject[ServerParams.X].f;
                float y = contentObject[ServerParams.Y].f;
                listener.PickWeaponReceived(player, new Vector2(x, y));
            }
        }
    }
}