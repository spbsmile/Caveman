using Caveman.Players;
using Caveman.UI;
using Caveman.Utils;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace Caveman.Network
{
    public class Multiplayer : EnterPoint, IServerListener
    {
        public Transform prefabServerPlayer;

        public const float WidthMapServer = 1350;
        public const float HeigthMapServer = 1350;
        private static string OwnId;
        private bool resultReceived;

        public override void Awake()
        {
            base.Awake();
            Setting.Settings.multiplayerMode = true;
        }

        public override void Start()
        {
            OwnId = SystemInfo.deviceUniqueIdentifier;
            serverConnection = new ServerConnection {ServerListener = this};
            serverConnection.StartSession(OwnId,
                PlayerPrefs.GetString(AccountManager.KeyNickname));
            base.Start();
            serverConnection.SendRespawn(poolPlayers[OwnId].transform.position);
        }

        public void Update()
        {
            if (!resultReceived) serverConnection.Update();
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

        //public void WeaponRemovedReceived(string key)
        //{
        //    poolStones.Store(key);
        //}

        public void WeaponPickReceived(string playerId, string key)
        {
            print("sfsdfhlsdkjfhsldkjhljk");
            poolPlayers[playerId].PickupWeapon(poolStones[key]);
        }

        public void WeaponUseReceived(string playerId, Vector2 aim)
        {
            poolPlayers[playerId].Throw(aim);
        }

        public void BonusAddedReceived(string key, Vector2 point)
        {
            poolBonusesSpeed.New(key).transform.position = point;
        }

        public void BonusPickReceived(string playerId, string key)
        {
            poolPlayers[playerId].PickupBonus(poolBonusesSpeed[key]);
        }

        public void PlayerRespawnReceived(string playerId, Vector2 point)
        {
            if (!poolPlayers.ContainsKey(playerId))
            {
                CreatePlayer(new Player("No Name"), playerId, false, true, prefabServerPlayer);
                poolPlayers[playerId].transform.position = point;
                poolPlayers[playerId].firstRespawn = false;
            }
            else if (poolPlayers[playerId].firstRespawn)
            {
                poolPlayers[playerId].firstRespawn = false;
                poolPlayers[playerId].transform.position = point;
            }
            else
            {
                StartCoroutine(poolPlayers[playerId].Respawn(point));
            }
        }

        public void PlayerDeadReceived(string playerId)
        {
            poolPlayers[playerId].Die();
        }

        public void PlayerMoveReceived(string playerId, Vector2 point)
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

        public void GameTimeReceived(float time)
        {
            StartCoroutine(BattleGui.instance.mainGameTimer.UpdateTime((int)time));
        }

        public void GameResultReceived(JEnumerable<JObject> data)
        {
            var resultRound = BattleGui.instance.resultRound;
            resultRound.gameObject.SetActive(true);
            var lineIndex = 0;
            foreach (var jsonObject in data)
            {
                resultRound.Write(jsonObject[ServerParams.UserName].ToString(), resultRound.names, lineIndex);
                resultRound.Write(jsonObject[ServerParams.Kills].ToString(), resultRound.kills, lineIndex);
                resultRound.Write(jsonObject[ServerParams.Deaths].ToString(), resultRound.deaths, lineIndex);
                lineIndex++;
            }
            resultReceived = true;
        }

        public void LoginReceived(string playerId, string playerName)
        {
            CreatePlayer(new Player(playerName), playerId, false, true, prefabServerPlayer);
            serverConnection.SendRespawn(poolPlayers[OwnId].transform.position);
        }

        public void LogoutReceived(string playerId)
        {
            var gameObject = poolPlayers[playerId].gameObject;
            poolPlayers.Remove(playerId);
            Destroy(gameObject);
        }

        public void OnDestroy()
        {
            serverConnection.StopSession();
        }
    }
}
