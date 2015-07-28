using System;
using Caveman.Setting;
using UnityEngine;

namespace Caveman.Network
{
    public class ClientMessage
    {
        private ClientMessage(JSONObject jsonContent)
        {
            this.jsonContent = jsonContent;
            Content = ContentFromJson();
        }

        public string Content { get; private set; }

        private JSONObject jsonContent;

        public void AddParam(string key, string value)
        {
            if (jsonContent == null)
            {
                jsonContent = new JSONObject(JSONObject.Type.OBJECT);
            }
            jsonContent.AddField(key, value);
            Content = ContentFromJson();
        }

        public static ClientMessage LoginMessage(string userName, string playerId)
        {
            var json = new JSONObject(JSONObject.Type.OBJECT);
            json.AddField(ServerParams.ActionType, "login");
            json.AddField(ServerParams.UserName, userName);
            return new ClientMessage(json);
        }

        public static ClientMessage TickMessage()
        {
            var json = new JSONObject(JSONObject.Type.OBJECT);
            json.AddField(ServerParams.ActionType, ServerParams.PingAction);
            return new ClientMessage(json);
        }

        public static ClientMessage PickWeapon(string weaponId)
        {
            var pointServer = GetServerPoint(weaponId);
            var json = new JSONObject(JSONObject.Type.OBJECT);
            json.AddField(ServerParams.ActionType, ServerParams.PickWeaponAction);
            json.AddField(ServerParams.X, pointServer.x);
            json.AddField(ServerParams.Y, pointServer.y);
            return new ClientMessage(json);
        }

        public static ClientMessage UseWeapon(float x, float y)
        {
            var json = new JSONObject(JSONObject.Type.OBJECT);
            json.AddField(ServerParams.ActionType, ServerParams.UseWeaponAction);
            json.AddField(ServerParams.X, x);
            json.AddField(ServerParams.Y, y);
            return new ClientMessage(json);
        }

        public static ClientMessage PickBonus(string bonusId)
        {
            var pointServer = GetServerPoint(bonusId);
            var json = new JSONObject(JSONObject.Type.OBJECT);
            json.AddField(ServerParams.ActionType, ServerParams.PickBonusAction);
            json.AddField(ServerParams.X, pointServer.x);
            json.AddField(ServerParams.Y, pointServer.y);
            return new ClientMessage(json);
        }

        public static ClientMessage Respawn(string playerId, Vector2 pointClient)
        {
            var pointServer = GetServerPoint(pointClient);
            var json = new JSONObject(JSONObject.Type.OBJECT);
            json.AddField(ServerParams.ActionType, ServerParams.RespawnAction);
            json.AddField(ServerParams.X, pointServer.x);
            json.AddField(ServerParams.Y, pointServer.y);
            json.AddField(ServerParams.UserId, playerId);
            return new ClientMessage(json);
        }

        public static ClientMessage PlayerDead()
        {
            var json = new JSONObject(JSONObject.Type.OBJECT);
            json.AddField(ServerParams.ActionType, ServerParams.PlayerDeadAction);
            return new ClientMessage(json);
        }

        public static ClientMessage Move(float x, float y)
        {
            var json = new JSONObject(JSONObject.Type.OBJECT);
            json.AddField(ServerParams.ActionType, ServerParams.MoveAction);
            json.AddField(ServerParams.X, x);
            json.AddField(ServerParams.Y, y);
            return new ClientMessage(json);
        }

        public static ClientMessage Logout(string playerId)
        {
            var json = new JSONObject(JSONObject.Type.OBJECT);
            json.AddField(ServerParams.ActionType, ServerParams.LogoutAction);
            json.AddField(ServerParams.UserId, playerId);
            return new ClientMessage(json);
        }

        private string ContentFromJson()
        {
            return jsonContent != null ? "#" + jsonContent + "#" : "";
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