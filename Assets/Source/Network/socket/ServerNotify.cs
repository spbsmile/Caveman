using System;
using Caveman.Setting;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace Caveman.Network
{
    public class ServerNotify : ServerConnection, IClientListener
    {
        public void LoginMessage(string userName)
        {
            throw new NotImplementedException();
        }

        public void TickMessage()
        {
            throw new NotImplementedException();
        }

        public void PickWeapon(string weaponId)
        {
            throw new NotImplementedException();
        }

        public void UseWeapon(Vector2 pointClient)
        {
            var pointServer = GetServerPoint(pointClient);

            SendMessageToSocket(ParseContentForServer(new JObject
            {
                {ServerParams.ActionType, ServerParams.BonusPickAction},
                {ServerParams.X, pointServer.x},
                {ServerParams.Y, pointServer.y}
            }));
        }

        public void PickBonus(string bonusId)
        {
            throw new NotImplementedException();
        }

        public void PlayerGold(int gold)
        {
            throw new NotImplementedException();
        }

        public void Respawn(Vector2 pointClient)
        {
            throw new NotImplementedException();
        }

        public void PlayerDead()
        {
            throw new NotImplementedException();
        }

        public void AddedKillStat(string killerId)
        {
            throw new NotImplementedException();
        }

        public void Move(Vector2 pointClient)
        {
            throw new NotImplementedException();
        }

        public void Logout()
        {
            throw new NotImplementedException();
        }

        private string ParseContentForServer(JObject jObject)
        {
            return jObject != null ? "#" + jObject + "#" : "";
        }

        private static Vector2 GetServerPoint(string id)
        {
            var index = id.IndexOf(":");
            var x = id.Substring(0, index);
            var y = id.Substring(index + 1);
            return new Vector2(Convert.ToInt32(x), Convert.ToInt32(y));
        }

        private static Vector2 GetServerPoint(Vector2 pointClient)
        {
            var x = (pointClient.x / Settings.WidthMap) * Multiplayer.WidthMapServer;
            var y = (pointClient.y / Settings.HeightMap) * Multiplayer.HeigthMapServer;
            return new Vector2(x, y);
        }
    }
}
