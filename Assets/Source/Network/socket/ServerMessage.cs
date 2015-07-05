using System;
using System.IO;
using UnityEngine;

namespace Caveman.Network
{
    public class ServerMessage
    {
        private const string MESSAGE_MARKER_START = "#";
        private const string MESSAGE_MARKER_END = "&";

        private readonly JSONObject contentObject;

        public ServerMessage(string content)
        {
            contentObject = new JSONObject(content);
        }

        public void SendMessageToListener(IServerListener listener)
        {
            if (contentObject.IsArray)
            {
                foreach (JSONObject jsonItem in contentObject.list)
                {
                    string actionType = jsonItem.GetField(ServerParams.ACTION_TYPE).str;
                    SendMessageToListener(listener, jsonItem, actionType);
                }
            }
            else
            {
                string actionType = contentObject.GetField(ServerParams.ACTION_TYPE).ToString();
                SendMessageToListener(listener, contentObject, actionType);
            }
        }

        public void SendMessageToListener(IServerListener listener, JSONObject action, string actionType)
        {
            if (actionType.Equals(ServerParams.STONE_ADDED_ACTION))
            {
                float x = action[ServerParams.X].f;
                float y = action[ServerParams.Y].f;
                listener.StoneAddedReceived(new Vector2(x, y));
            }
            else if (actionType.Equals(ServerParams.STONE_REMOVED_ACTION))
            {
                float x = action[ServerParams.X].f;
                float y = action[ServerParams.Y].f;
                listener.StoneRemovedReceived(new Vector2(x, y));
            }
            else if (actionType.Equals(ServerParams.MOVE_ACTION))
            {
                string player = action[ServerParams.PLAYER].str;
                float x = action[ServerParams.X].f;
                float y = action[ServerParams.Y].f;
                listener.MoveReceived(player, new Vector2(x, y));
            }
            else if (actionType.Equals(ServerParams.PICK_BONUS_ACTION))
            {
                string player = action[ServerParams.PLAYER].str;
                float x = action[ServerParams.X].f;
                float y = action[ServerParams.Y].f;
                listener.PickBonusReceived(player, new Vector2(x, y));
            }
            else if (actionType.Equals(ServerParams.PICK_WEAPON_ACTION))
            {
                string player = action[ServerParams.PLAYER].str;
                float x = action[ServerParams.X].f;
                float y = action[ServerParams.Y].f;
                listener.PickWeaponReceived(player, new Vector2(x, y));
            }
        }
    }
}