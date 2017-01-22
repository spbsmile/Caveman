﻿using System;
using Caveman.Setting;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace Caveman.Network
{
    public class  ServerNotify : ServerConnection, IServerNotify
    {
        public void PickWeaponSend(string weaponId, int type)
        {
            var pointServer = GetServerPoint(weaponId);
            SendMessageToSocket(new JObject
            {
                {ServerParams.ActionType, ServerParams.WeaponPickAction},
                {ServerParams.X, pointServer.x},
                {ServerParams.Y, pointServer.y},
            });
        }

        public void ActivateWeaponSend(Vector2 pointClient, int type)
        {
            var pointServer = GetServerPoint(pointClient);
            SendMessageToSocket(new JObject
            {
                {ServerParams.ActionType, ServerParams.BonusPickAction},
                {ServerParams.X, pointServer.x},
                {ServerParams.Y, pointServer.y}
            });
        }

        public void PickBonusSend(string bonusId, int type)
        {
            var pointServer = GetServerPoint(bonusId);
            SendMessageToSocket(new JObject
            {
                {ServerParams.ActionType, ServerParams.BonusPickAction},
                {ServerParams.X, pointServer.x},
                {ServerParams.Y, pointServer.y}
            });
        }

        // TODO: set gold player on server
        public void PlayerGold(int gold)
        {
            SendMessageToSocket(new JObject
            {
               
            });
        }

        public void RespawnSend(Vector2 pointClient)
        {
            var pointServer = GetServerPoint(pointClient);
            SendMessageToSocket(new JObject
            {
                {ServerParams.ActionType, ServerParams.PlayerRespawnAction},
                {ServerParams.X, pointServer.x},
                {ServerParams.Y, pointServer.y}
            });
        }

        public void PlayerDeadSend()
        {
            SendMessageToSocket(new JObject
            {
                 {ServerParams.ActionType, ServerParams.PlayerDeadAction}
            });
        }

        public void AddedKillStatSend(string killerId)
        {
            SendMessageToSocket(new JObject
            {
                {ServerParams.ActionType, ServerParams.DeadAction},
                {ServerParams.Killer, killerId},
            });
        }

        public void MoveSend(Vector2 pointClient)
        {
            var pointServer = GetServerPoint(pointClient);
            SendMessageToSocket(new JObject
            {
                {ServerParams.ActionType, ServerParams.PlayerMoveAction},
                {ServerParams.X, pointServer.x},
                {ServerParams.Y, pointServer.y}
            });
        }

        public void LogoutSend()
        {
            SendMessageToSocket(new JObject
            {
                {ServerParams.ActionType, ServerParams.LogoutAction}
            });
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
            var x = (pointClient.x / Settings.WidthMap) * ServerMessageHandler.WidthMapServer;
            var y = (pointClient.y / Settings.HeightMap) * ServerMessageHandler.HeigthMapServer;
            return new Vector2(x, y);
        }
    }
}