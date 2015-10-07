using System.Linq;
using Caveman.Setting;
using Newtonsoft.Json.Linq;
using UnityEngine;

    
namespace Caveman.Network
{
    public class ServerMessage
    {
        private readonly JArray jArray;

        public ServerMessage(string content)
        {
            jArray = JArray.Parse(content);
        }

        public void SendMessageToListener(IServerListener listener)
        {
            //todo & 
            if (jArray != null)
            {
                foreach (var jToken in jArray)
                {
                    if (jToken[ServerParams.ActionType].ToString()!= "move")
                    {
                        //Debug.Log(jToken[ServerParams.ActionType].ToString());
                    }
                    SendMessageToListener(listener, jToken, jToken.Value<string>(ServerParams.ActionType));
                }
            }
            //else
            //{
            //    SendMessageToListener(listener, jArray, jArray[ServerParams.ActionType].ToString());
            //}
        }


        private void SendMessageToListener(IServerListener listener, JToken jToken, string action)
        {
            var pointServer = (jToken[ServerParams.X] != null && jToken[ServerParams.Y] != null)
                ? new Vector2(jToken.Value<float>(ServerParams.X), jToken.Value<float>(ServerParams.Y))
                : Vector2.zero;
            var key = GenerateKey(pointServer);

            var pointClient = Convector(pointServer);
            var playerId = jToken.Value<string>(ServerParams.UserId);

            if (action.Equals(ServerParams.StoneAddedAction))
            {

                listener.WeaponAddedReceived(key, pointClient);
            }
            //else if (action.Equals(ServerParams.StoneRemovedAction))
            //{
            //    listener.WeaponRemovedReceived(key);
            //}
            else if (action.Equals(ServerParams.PlayerMoveAction))
            {
                listener.PlayerMoveReceived(playerId, pointClient);
            }
            else if (action.Equals(ServerParams.WeaponPickAction))
            {
                listener.WeaponPickReceived(playerId, key);
            }
            else if (action.Equals(ServerParams.BonusAddedAction))
            {
                listener.BonusAddedReceived(key, pointClient);
            }
            else if (action.Equals(ServerParams.BonusPickAction))
            {
                listener.BonusPickReceived(playerId, key);
            }
            else if (action.Equals(ServerParams.UseWeaponAction))
            {                                        //aim
                listener.WeaponUseReceived(playerId, pointClient);
            }
            else if (action.Equals(ServerParams.PlayerRespawnAction))
            {
                listener.PlayerRespawnReceived(playerId, pointClient);
            }
            else if (action.Equals(ServerParams.LoginAction))
            {
                listener.LoginReceived(playerId, jToken.Value<string>(ServerParams.UserName));
            }
            else if (action.Equals(ServerParams.PlayerDeadAction))
            {
                listener.PlayerDeadReceived(playerId);
            }
            else if (action.Equals(ServerParams.GameTimeAction))
            {
                listener.GameTimeReceived(jToken.Value<float>(ServerParams.GameTimeLeft));
            }
            else if (action.Equals(ServerParams.LogoutAction))
            {
                listener.LogoutReceived(playerId);
            }
            else if (action.Equals(ServerParams.GameResultAction))
            {
                listener.GameResultReceived(jToken[ServerParams.Data]);
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