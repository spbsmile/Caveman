using System.Collections.Generic;
using Caveman.Players;
using Caveman.UI;
using Caveman.Utils;
using UnityEngine;

namespace Caveman.Network
{
    public class Multiplayer : EnterPoint, IServerListener
    {
        public Transform prefabServerPlayer;

        public const float WidthMapServer = 1350;
        public const float HeigthMapServer = 1350;
        private static string OwnId;

        public override void Awake()
        {
            Setting.Settings.multiplayerMode = true;
        }

        public override void Start()
        {
            OwnId = SystemInfo.deviceUniqueIdentifier;
            serverConnection = new ServerConnection {ServerListener = this};
            serverConnection.StartSession(OwnId,
                PlayerPrefs.GetString(AccountManager.KeyNickname));
            base.Start();
            serverConnection.SendRespawn(OwnId,
                poolPlayers[OwnId].transform.position);
        }

        public void Update()
        {
            serverConnection.Update();
        }

        public void WeaponAddedReceived(string key, Vector2 point)
        {
            if (!poolStones.ContainsKey(key))
            {
                poolStones.New(key).transform.position = point;     
            }
            else
            {
                Debug.LogWarning(key + " An element with the same key already exists in the dictionary.");
            }
        }

        public void BonusAddedReceived(string key, Vector2 point)
        {

        }

        public void PlayerDeadReceived(string playerId)
        {
            poolPlayers[playerId].Die();
        }

        public void ResultReceived(List<JSONObject> data)
        {
            var resultRound = BattleGui.instance.resultRound;
            resultRound.gameObject.SetActive(true);
            var lineIndex = 0;
            foreach (var jsonObject in data)
            {
                resultRound.Write(jsonObject[ServerParams.UserName].str, resultRound.names, lineIndex);
                resultRound.Write(jsonObject[ServerParams.Kills].n.ToString(), resultRound.kills, lineIndex);
                resultRound.Write(jsonObject[ServerParams.Deaths].n.ToString(), resultRound.deaths, lineIndex);
                lineIndex++;
            }
        }

        public void TimeReceived(float time)
        {
            StartCoroutine(BattleGui.instance.mainGameTimer.UpdateTime((int)time));
        }

        public void WeaponRemovedReceived(string key)
        {
            poolStones.Store(key);
        }

        public void MoveReceived(string playerId, Vector2 point)
        {
            if (poolPlayers.ContainsKey(playerId))
            {
                var playerServer = poolPlayers[playerId];
                if (Vector2.SqrMagnitude((Vector2)playerServer.transform.position - point) < UnityExtensions.ThresholdPosition)
                {
                    playerServer.StopMove();
                }
                else
                {
                    playerServer.SetMove(point);
                }
            }
            else
            {
                Debug.LogWarning("Player null, but move received invoke");
            }
        }

        public void LoginReceived(string playerId, string playerName, Vector2 position)
        {
            CreatePlayer(new Player(playerName), playerId, false, true, prefabServerPlayer);
            serverConnection.SendRespawn(OwnId,
                poolPlayers[OwnId].transform.position);
        }

        public void LogoutReceived(string playerId)
        {
            var gameObject = poolPlayers[playerId].gameObject;
            poolPlayers.Remove(playerId);
            Destroy(gameObject);
        }

        public void PickWeaponReceived(string playerId, string key)
        {
            poolPlayers[playerId].PickupWeapon(poolStones[key]);
        }

        public void PickBonusReceived(string playerId, string key)
        {
            poolPlayers[playerId].PickupBonus(poolBonusesSpeed[key]);
        }

        public void UseWeaponReceived(string playerId, Vector2 aim)
        {
            poolPlayers[playerId].Throw(aim);
        }

        public void RespawnReceived(string playerId, Vector2 point)
        {
            if (!poolPlayers.ContainsKey(playerId))
            {
                CreatePlayer(new Player("No Name"), playerId, false, true, prefabServerPlayer);
                poolPlayers[playerId].transform.position = point;
            }
            else if (!poolPlayers[playerId].inGame)
            {
                poolPlayers[playerId].inGame = true;
                poolPlayers[playerId].transform.position = point;
            }
            else
            {
                StartCoroutine(poolPlayers[playerId].Respawn(point));
            }
        }

        public void OnDestroy()
        {
            serverConnection.StopSession();
        }
    }
}
