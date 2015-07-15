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

        public static ClientMessage LoginMessage(string userName)
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

        public static ClientMessage PickWeapon(float x, float y)//string weaponId)
        {
            //x = 
            //y = 
            var json = new JSONObject(JSONObject.Type.OBJECT);
            json.AddField(ServerParams.ActionType, ServerParams.PickWeaponAction);
            json.AddField(ServerParams.X, x);
            json.AddField(ServerParams.Y, y);
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

        public static ClientMessage PickBonus(float x, float y)
        {
            var json = new JSONObject(JSONObject.Type.OBJECT);
            json.AddField(ServerParams.ActionType, ServerParams.PickBonusAction);
            json.AddField(ServerParams.X, x);
            json.AddField(ServerParams.Y, y);
            return new ClientMessage(json);
        }

        public static ClientMessage Respawn(float x, float y)
        {
            var json = new JSONObject(JSONObject.Type.OBJECT);
            json.AddField(ServerParams.ActionType, ServerParams.RespawnAction);
            json.AddField(ServerParams.X, x);
            json.AddField(ServerParams.Y, y);
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

        private string ContentFromJson()
        {
            return jsonContent != null ? "#" + jsonContent + "#" : "";
        }
    }
}