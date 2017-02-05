using Caveman.Setting;
using Newtonsoft.Json.Linq;
using UnityEngine;

    
namespace Caveman.Network
{
    public class ServerMessageManager
    {
        private readonly JArray jArray;

        public ServerMessageManager(string content)
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
                        //Debug.Log(jToken);
                        Debug.Log(jToken[ServerParams.ActionType].ToString());
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
            if (action.Equals(ServerParams.GameInfoAction))
            {
                var map = jToken["map"] as JObject;
                listener.GameInfoMapReceive(map);
                listener.GameInfoPlayersReceive(jToken["players"]);
            }
            else
            {
                var pointServer = (jToken[ServerParams.X] != null && jToken[ServerParams.Y] != null)
                    ? new Vector2(jToken.Value<float>(ServerParams.X), jToken.Value<float>(ServerParams.Y))
                    : Vector2.zero;
                var key = GenerateKey(pointServer);

                var pointClient = Convector(pointServer);
                var playerId = jToken.Value<string>(ServerParams.UserId);

                if (action.Equals(ServerParams.StoneAddedAction))
                {
                    listener.WeaponAddedReceive(key, pointClient);
                }
                else if (action.Equals(ServerParams.StoneRemovedAction))
                {
                    listener.WeaponRemovedReceive(key);
                }
                else if (action.Equals(ServerParams.PlayerMoveAction))
                {
                    listener.PlayerMoveReceive(playerId, pointClient);
                }
                else if (action.Equals(ServerParams.WeaponPickAction))
                {
                    listener.WeaponPickReceive(playerId, key);
                }
                else if (action.Equals(ServerParams.BonusAddedAction))
                {
                    listener.BonusAddedReceive(key, pointClient);
                }
                else if (action.Equals(ServerParams.BonusRemovedAction))
                {
                    listener.BonusRemovedReceive(key, pointClient);
                }
                else if (action.Equals(ServerParams.BonusPickAction))
                {
                    listener.BonusPickReceive(playerId, key);
                }
                else if (action.Equals(ServerParams.UseWeaponAction))
                {                                        //aim
                    listener.WeaponUseReceive(playerId, pointClient);
                }
                else if (action.Equals(ServerParams.PlayerRespawnAction))
                {
                    listener.PlayerRespawnReceive(playerId, pointClient);
                }
                else if (action.Equals(ServerParams.LoginAction))
                {
                    listener.LoginReceive(playerId, jToken.Value<string>(ServerParams.UserName));
                }
                else if (action.Equals(ServerParams.PlayerDeadAction))
                {
                    listener.PlayerDeadReceive(playerId);
                }
                else if (action.Equals(ServerParams.GameTimeAction))
                {
                    listener.GameTimeReceive(jToken.Value<float>(ServerParams.GameTimeLeft));
                }
                else if (action.Equals(ServerParams.LogoutAction))
                {
                    listener.LogoutReceive(playerId);
                }
                else if (action.Equals(ServerParams.GameResultAction))
                {
                    listener.GameResultReceive(jToken[ServerParams.Data]);
                }
            }
        }

        private string GenerateKey(Vector2 point)
        {
            return point.x + ":" + point.y;
        }

        private Vector2 Convector(Vector2 pointServer)
        {                                    
            return new Vector2(pointServer.x / ServerMessageHandler.MapServerConfig.Width * Settings.WidthMap,
             pointServer.y / ServerMessageHandler.MapServerConfig.Heght * Settings.HeightMap);
        }
    }
}