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

        private string ContentFromJson()
        {
            return jObject != null ? "#" + jObject + "#" : "";
        }
    }
}