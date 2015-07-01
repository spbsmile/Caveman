
using System.Collections.Generic;

namespace Caveman.Network
{
    public class ClientMessage
    {
        public ushort Length { get; set; }
        public string Content { get; set; }

        //private functions

        public static ClientMessage LoginMessage(IDictionary<string,string> actionParams)
        {
            var userName = actionParams[ServerParams.USER_NAME];

            var json = new JSONObject(JSONObject.Type.OBJECT);
            json.AddField(ServerParams.ACTION_TYPE,"login");
            json.AddField(ServerParams.USER_ID,1);
            json.AddField(ServerParams.USER_NAME, userName);
            return new ClientMessage("#" + json + "#");
        }

        public static ClientMessage TickMessage()
        {
            var json = new JSONObject(JSONObject.Type.OBJECT);
            json.AddField(ServerParams.ACTION_TYPE,"tick");
            return new ClientMessage("#" + json + "#");
        }

        public static ClientMessage PickWeapon(float x, float y)
        {
            var json = new JSONObject(JSONObject.Type.OBJECT);
            json.AddField(ServerParams.ACTION_TYPE, ServerParams.PICK_WEAPON_ACTION);
            json.AddField(ServerParams.X, x);
            json.AddField(ServerParams.Y, y);
            return new ClientMessage("#" + json + "#");
        }

        public static ClientMessage UseWeapon(float x, float y)
        {
            var json = new JSONObject(JSONObject.Type.OBJECT);
            json.AddField(ServerParams.ACTION_TYPE, ServerParams.USE_WEAPON_ACTION);
            json.AddField(ServerParams.X, x);
            json.AddField(ServerParams.Y, y);
            return new ClientMessage("#" + json + "#");
        }

        public static ClientMessage PickBonus(float x, float y)
        {
            var json = new JSONObject(JSONObject.Type.OBJECT);
            json.AddField(ServerParams.ACTION_TYPE, ServerParams.PICK_BONUS_ACTION);
            json.AddField(ServerParams.X, x);
            json.AddField(ServerParams.Y, y);
            return new ClientMessage("#" + json + "#");
        }


        private ClientMessage(string content)
        {
            Content = content;
        }

    }
}