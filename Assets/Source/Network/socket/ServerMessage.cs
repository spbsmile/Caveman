using UnityEngine;

namespace Caveman.Network
{
    public class ServerMessage
    {
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
                    SendMessageToListener(listener, jsonItem, jsonItem.GetField(ServerParams.ActionType).str);
                }
            }
            else
            {
                SendMessageToListener(listener, contentObject, contentObject.GetField(ServerParams.ActionType).ToString());
            }
        }


        private void SendMessageToListener(IServerListener listener, JSONObject action, string type)
        {
            var point = new Vector2(action[ServerParams.X].f, action[ServerParams.Y].f);
            var playerId = action[ServerParams.Player]!= null ?action[ServerParams.Player].str: null;
            if (type.Equals(ServerParams.StoneAddedAction))
            {
                listener.WeaponAddedReceived(point);
            }
            else if (type.Equals(ServerParams.StoneRemovedAction))
            {
                listener.WeaponRemovedReceived(point);
            }
            else if (type.Equals(ServerParams.MoveAction))
            {
                listener.MoveReceived(playerId, point);
            }
            else if (type.Equals(ServerParams.PickWeaponAction))
            {
                listener.PickWeaponReceived(playerId, point);
            }
            else if (type.Equals(ServerParams.BonusAddedAction))
            {
                listener.BonusAddedReceived(point);
            }
            else if (type.Equals(ServerParams.PickBonusAction))
            {
                listener.PickBonusReceived(playerId, point);
            }
            else if (type.Equals(ServerParams.UseWeaponAction))
            {
                listener.UseWeaponReceived(playerId, point);
            }
            else if (type.Equals(ServerParams.RespawnAction))
            {
                listener.RespawnReceived(playerId, point);
            } 
            //todo ServerParams.LoginAction LogoutAction &
            else if (type.Equals(ServerParams.LoginAction))
            {
                listener.LoginReceived(playerId);
            }
            else if (type.Equals(ServerParams.PlayerDeadAction))
            {
                listener.PlayerDeadResceived(playerId, point);
            }
        }
    }
}