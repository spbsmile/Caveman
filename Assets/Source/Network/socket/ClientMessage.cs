using System;
using Caveman.Setting;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace Caveman.Network
{
    public class ClientMessage
    {
        private ClientMessage(JObject jObject)
        {
            this.jObject = jObject;
            Content = ContentFromJson();
        }

        public string Content { get; private set; }

        private JObject jObject;

        public void AddParam(string key, string value)
        {
            if (jObject == null)
            {
                jObject = new JObject();
            }
            jObject.Add(key, value);
            Content = ContentFromJson();
        }

        public static ClientMessage LoginMessage(string userName)
        {
            return new ClientMessage(new JObject
            {
                {ServerParams.ActionType, ServerParams.LoginAction},
                {ServerParams.UserName, userName}
            });
        }

        public static ClientMessage TickMessage()
        {
            return new ClientMessage(new JObject
            {
                {ServerParams.ActionType, ServerParams.PingAction}
            });
        }

        public static ClientMessage PickWeapon(string weaponId)
        {
            var pointServer = GetServerPoint(weaponId);
            return new ClientMessage(new JObject
            {
                {ServerParams.ActionType, ServerParams.WeaponPickAction},
                {ServerParams.X, pointServer.x},
                {ServerParams.Y, pointServer.y},
            });
        }

        public static ClientMessage UseWeapon(Vector2 pointClient)
        {
            var pointServer = GetServerPoint(pointClient);
            return new ClientMessage(new JObject
            {
                {ServerParams.ActionType, ServerParams.UseWeaponAction},
                {ServerParams.X, pointServer.x},
                {ServerParams.Y, pointServer.y}
            });
        }

        public static ClientMessage PickBonus(string bonusId)
        {
            var pointServer = GetServerPoint(bonusId);
            return new ClientMessage(new JObject
            {
                {ServerParams.ActionType, ServerParams.BonusPickAction},
                {ServerParams.X, pointServer.x},
                {ServerParams.Y, pointServer.y}
            });
        }

        public static ClientMessage Respawn(Vector2 pointClient)
        {
            var pointServer = GetServerPoint(pointClient);
            return new ClientMessage(new JObject
            {
                {ServerParams.ActionType, ServerParams.PlayerRespawnAction},
                {ServerParams.X, pointServer.x},
                {ServerParams.Y, pointServer.y}
            });
        }

        public static ClientMessage PlayerDead()
        {
            return new ClientMessage(new JObject
            {
                {ServerParams.ActionType, ServerParams.PlayerDeadAction}
            });
        }

        public static ClientMessage AddedKillStat(string killerId)
        {
            return new ClientMessage(new JObject
            {
                {ServerParams.ActionType, ServerParams.DeadAction},
                {ServerParams.Killer, killerId},
            });
        }

        public static ClientMessage Move(Vector2 pointClient)
        {
            var pointServer = GetServerPoint(pointClient);
            return new ClientMessage(new JObject
            {
                {ServerParams.ActionType, ServerParams.PlayerMoveAction},
                {ServerParams.X, pointServer.x},
                {ServerParams.Y, pointServer.y}
            });
        }

        public static ClientMessage Logout()
        {
            return new ClientMessage(new JObject
            {
                {ServerParams.ActionType, ServerParams.LogoutAction}
            });
        }

        private string ContentFromJson()
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