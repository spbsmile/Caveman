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
                    //if (jsonItem[ServerParams.ActionType].str != "move")
                    //{
                    //    Debug.Log(contentObject.ToString());
                    //}
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
                listener.PlayerDeadReceived(playerId);
            }
            else if (type.Equals(ServerParams.TimeAction))
            {
                listener.TimeReceived(action[ServerParams.TimeLeft].f);
            }
            else if (type.Equals(ServerParams.LogoutAction))
            {
                listener.LogoutReceived(playerId);
            }
            else if (type.Equals(ServerParams.ResultAction))
            {
                Debug.Log(action[ServerParams.Player].str);
                Debug.Log(action[ServerParams.UserName].str);
                Debug.Log(action[ServerParams.ResultAction].str);

                listener.ResultReceived(" ResultReceived");
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