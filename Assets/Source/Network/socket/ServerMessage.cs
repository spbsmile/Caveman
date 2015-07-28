using Caveman.Setting;
using UnityEngine;

namespace Caveman.Network
{
    public class ServerMessage
    {
        private readonly JSONObject contentObject;

        public ServerMessage(string content)
        {
            if (content != "[]")
            {
                Debug.Log("from server " + content);    
            }
            contentObject = new JSONObject(content);
        }

        public void SendMessageToListener(IServerListener listener)
        {
            if (contentObject.IsArray)
            {
                foreach (var jsonItem in contentObject.list)
                {
                    SendMessageToListener(listener, jsonItem, jsonItem[ServerParams.ActionType].str);
                }
            }
            else
            {
                SendMessageToListener(listener, contentObject, contentObject[ServerParams.ActionType].str);
            }
        }


        private void SendMessageToListener(IServerListener listener, JSONObject action, string type)
        {
            var pointServer = (action[ServerParams.X] != null && action[ServerParams.Y] != null)
                ? new Vector2(action[ServerParams.X].f, action[ServerParams.Y].f)
                : Vector2.zero;
            var key = GenerateKey(pointServer);
            var pointClient = Convector(pointServer);
            //Debug.Log(pointServer + " pointServer");
            var playerId = action[ServerParams.UserId]!= null ?action[ServerParams.UserId].str: null;
            if (type.Equals(ServerParams.StoneAddedAction))
            {
                listener.WeaponAddedReceived(key, pointClient);
            }
            else if (type.Equals(ServerParams.StoneRemovedAction))
            {
                listener.WeaponRemovedReceived(key);
            }
            else if (type.Equals(ServerParams.MoveAction))
            {
                listener.MoveReceived(playerId, pointClient);
            }
            else if (type.Equals(ServerParams.PickWeaponAction))
            {
                listener.PickWeaponReceived(playerId, key);
            }
            else if (type.Equals(ServerParams.BonusAddedAction))
            {
                listener.BonusAddedReceived(key, pointClient);
            }
            else if (type.Equals(ServerParams.PickBonusAction))
            {
                listener.PickBonusReceived(playerId, key);
            }
            else if (type.Equals(ServerParams.UseWeaponAction))
            {                                        //aim
                listener.UseWeaponReceived(playerId, pointClient);
            }
            else if (type.Equals(ServerParams.RespawnAction))
            {
                listener.RespawnReceived(playerId, pointClient);
            } 
            else if (type.Equals(ServerParams.LoginAction))
            {
                var playerName = action[ServerParams.UserName].str;
                listener.LoginReceived(playerId, playerName, pointClient);
            }
            else if (type.Equals(ServerParams.PlayerDeadAction))
            {
                listener.PlayerDeadResceived(playerId);
            }
            else if (type.Equals(ServerParams.TimeAction))
            {
                var time = action[ServerParams.TimeAction].str;
                listener.Time(time);
            }
            else if (type.Equals(ServerParams.LogoutAction))
            {
                listener.LogoutReceived(playerId);
            }
        }

        private string GenerateKey(Vector2 point)
        {
            return point.x + ":" + point.y;
        }

        private Vector2 Convector(Vector2 pointServer)
        {
            var x = (pointServer.x / Multiplayer.WidthMapServer) * Settings.WidthMap;
            var y = (pointServer.y / Multiplayer.HeigthMapServer) * Settings.HeightMap;
            return new Vector2(x, y);
        }
    }
}