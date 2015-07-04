using System.Collections.Generic;

namespace Caveman.Network
{
    public class ClientMessage
    {
        private ClientMessage(JSONObject jsonContent)
        {
            this.jsonContent = jsonContent;
            Content = ContentFromJson();
        }

        public ushort Length { get; set; }
        public string Content { get; set;}

        private JSONObject jsonContent;


        public void addParam(string paramKey, string paramValue)
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

        public static ClientMessage PickWeapon(float x, float y)
        {
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
    }
}