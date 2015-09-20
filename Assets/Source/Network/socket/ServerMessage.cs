using Caveman.Setting;
using UnityEngine;

namespace Caveman.Network
{
    public class ServerMessage
    {
        private readonly JSONObject contentObject;

        public ServerMessage(string content)
        {
            //if (content != "[]")
            //{
            //    Debug.Log("from server " + content);
            //}
            contentObject = new JSONObject(content);
        }

        public void SendMessageToListener(IServerListener listener)
        {
            if (contentObject.IsArray)
            {
                foreach (var jsonItem in contentObject.list)
                {
                    if (jsonItem[ServerParams.ActionType].str != "move")
                    {
                        Debug.Log(contentObject.ToString());
                    }
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
            //else if (type.Equals(ServerParams.StoneRemovedAction))
            //{
            //    listener.WeaponRemovedReceived(key);
            //}
            else if (type.Equals(ServerParams.PlayerMoveAction))
            {
                listener.PlayerMoveReceived(playerId, pointClient);
            }
            else if (type.Equals(ServerParams.WeaponPickAction))
            {
                listener.WeaponPickReceived(playerId, key);
            }
            else if (type.Equals(ServerParams.BonusAddedAction))
            {
                listener.BonusAddedReceived(key, pointClient);
            }
            else if (type.Equals(ServerParams.BonusPickAction))
            {
                listener.BonusPickReceived(playerId, key);
            }
            else if (type.Equals(ServerParams.UseWeaponAction))
            {                                        //aim
                listener.WeaponUseReceived(playerId, pointClient);
            }
            else if (type.Equals(ServerParams.PlayerRespawnAction))
            {
                listener.PlayerRespawnReceived(playerId, pointClient);
            } 
            else if (type.Equals(ServerParams.LoginAction))
            {
                var playerName = action[ServerParams.UserName].str;
                listener.LoginReceived(playerId, playerName);
            }
            else if (type.Equals(ServerParams.PlayerDeadAction))
            {
                listener.PlayerDeadReceived(playerId);
            }
            else if (type.Equals(ServerParams.GameTimeAction))
            {
                listener.GameTimeReceived(action[ServerParams.GameTimeLeft].f);
            }
            else if (type.Equals(ServerParams.LogoutAction))
            {
                listener.LogoutReceived(playerId);
            }
            else if (type.Equals(ServerParams.GameResultAction))
            {
                listener.GameResultReceived(action[ServerParams.Data].list);
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