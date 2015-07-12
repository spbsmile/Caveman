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

        public string Content { get; set; }

        private JSONObject jsonContent;

        public void AddParam(string paramKey, string paramValue)
        {
            if (jsonContent == null)
            {
                jsonContent = new JSONObject(JSONObject.Type.OBJECT);
            }
            jsonContent.AddField(paramKey, paramValue);
            Content = ContentFromJson();
        }

        public static ClientMessage LoginMessage(string userName)
        {
            var json = new JSONObject(JSONObject.Type.OBJECT);
            json.AddField(ServerParams.ACTION_TYPE, "login");
            json.AddField(ServerParams.USER_NAME, userName);
            return new ClientMessage(json);
        }

        public static ClientMessage TickMessage()
        {
            var json = new JSONObject(JSONObject.Type.OBJECT);
            json.AddField(ServerParams.ACTION_TYPE, ServerParams.PING_ACTION);
            return new ClientMessage(json);
        }

        public static ClientMessage PickWeapon(float x, float y)//string weaponId)
        {
            //x = 
            //y = 
            var json = new JSONObject(JSONObject.Type.OBJECT);
            json.AddField(ServerParams.ACTION_TYPE, ServerParams.PICK_WEAPON_ACTION);
            json.AddField(ServerParams.X, x);
            json.AddField(ServerParams.Y, y);
            return new ClientMessage(json);
        }

        public static ClientMessage UseWeapon(float x, float y)
        {
            var json = new JSONObject(JSONObject.Type.OBJECT);
            json.AddField(ServerParams.ACTION_TYPE, ServerParams.USE_WEAPON_ACTION);
            json.AddField(ServerParams.X, x);
            json.AddField(ServerParams.Y, y);
            return new ClientMessage(json);
        }

        public static ClientMessage PickBonus(float x, float y)
        {
            var json = new JSONObject(JSONObject.Type.OBJECT);
            json.AddField(ServerParams.ACTION_TYPE, ServerParams.PICK_BONUS_ACTION);
            json.AddField(ServerParams.X, x);
            json.AddField(ServerParams.Y, y);
            return new ClientMessage(json);
        }

        private string ContentFromJson()
        {
            return jsonContent != null ? "#" + jsonContent + "#" : "";
        }

        public static ClientMessage Respawn(string playerId)
        {
            var point = GetPointFromId(playerId);
            var json = new JSONObject(JSONObject.Type.OBJECT);
            json.AddField(ServerParams.ACTION_TYPE, ServerParams.RESPAWN_ACTION);
            json.AddField(ServerParams.X, point.x);
            json.AddField(ServerParams.Y, point.y);
            return new ClientMessage(json);
        }

        private static Vector2 GetPointFromId(string playerId)
        {
            throw new System.NotImplementedException();
        }

        public static ClientMessage PlayerDead(string playerId)
        {
            var point = GetPointFromId(playerId);
            var json = new JSONObject(JSONObject.Type.OBJECT);
            json.AddField(ServerParams.ACTION_TYPE, ServerParams.PLAYER_DEAD_ACTION);
            json.AddField(ServerParams.X, point.x);
            json.AddField(ServerParams.Y, point.y);
            return new ClientMessage(json);
        }
    }
}