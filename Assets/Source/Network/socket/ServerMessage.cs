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
            Debug.Log("from server " + content);
            contentObject = new JSONObject(content);
        }

        public void SendMessageToListener(IServerListener listener)
        {
            if (contentObject.IsArray)
            {
                foreach (var jsonItem in contentObject.list)
                {
                    var actionType = jsonItem.GetField(ServerParams.ACTION_TYPE).str;
                    SendMessageToListener(listener, jsonItem, actionType);
                }
            }
            else
            {
                var actionType = contentObject.GetField(ServerParams.ACTION_TYPE).ToString();
                SendMessageToListener(listener, contentObject, actionType);
            }
        }

        public void SendMessageToListener(IServerListener listener, JSONObject action, string actionType)
        {
            if (actionType.Equals(ServerParams.STONE_ADDED_ACTION))
            {
                listener.WeaponAddedReceived(new Vector2(action[ServerParams.X].f, action[ServerParams.Y].f));
            }
            else if (actionType.Equals(ServerParams.STONE_REMOVED_ACTION))
            {
                listener.WeaponRemovedReceived(new Vector2(action[ServerParams.X].f, action[ServerParams.Y].f));
            }
            else if (actionType.Equals(ServerParams.MOVE_ACTION))
            {
                var player = action[ServerParams.PLAYER].str;
                listener.MoveReceived(player, new Vector2(action[ServerParams.X].f, action[ServerParams.Y].f));
            }
            else if (actionType.Equals(ServerParams.PICK_BONUS_ACTION))
            {
                var player = action[ServerParams.PLAYER].str;
                listener.PickBonusReceived(player, new Vector2(action[ServerParams.X].f, action[ServerParams.Y].f));
            }
            else if (actionType.Equals(ServerParams.PICK_WEAPON_ACTION))
            {
                var player = action[ServerParams.PLAYER].str;
                listener.PickWeaponReceived(player, new Vector2(action[ServerParams.X].f, action[ServerParams.Y].f));
            }
            else if (action.Equals(ServerParams.BONUS_ADDED_ACTION))
            {
                listener.BonusAddedReceived(new Vector2(action[ServerParams.X].f, action[ServerParams.Y].f));
            }
        }
    }
}