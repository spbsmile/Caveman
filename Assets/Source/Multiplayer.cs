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

        //todo this const - temp. Defined on server 
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
            if (RepeatKey(poolStones, key)) return;
            poolStones.New(key).transform.position = point;
        }

        public void WeaponRemovedReceived(string key)
        {
            poolStones.Store(key);
        }

        public void WeaponPickReceived(string playerId, string key)
        {
            poolPlayers[playerId].PickupWeapon(poolStones[key]);
        }

        public void WeaponUseReceived(string playerId, Vector2 aim)
        {
            poolPlayers[playerId].Throw(aim);
        }

        public void BonusAddedReceived(string key, Vector2 point)
        {
            if (RepeatKey(poolBonusesSpeed, key)) return;
            poolBonusesSpeed.New(key).transform.position = point;    
        }

        public void BonusRemovedReceived(string key, Vector2 point)
        {
            poolBonusesSpeed.Store(key);
        }

        public void BonusPickReceived(string playerId, string key)
        {
            poolPlayers[playerId].PickupBonus(poolBonusesSpeed[key]);
        }

        public void PlayerRespawnReceived(string playerId, Vector2 point)
        {
            if (poolPlayers[playerId].firstRespawn)
            {
                poolPlayers[playerId].firstRespawn = false;
                poolPlayers[playerId].transform.position = point;
            }
            else
            {
                poolPlayers[playerId].Birth(point);
            }
        }

        public void PlayerDeadReceived(string playerId)
        {
            poolPlayers[playerId].Die();
        }

        public void PlayerMoveReceived(string playerId, Vector2 point)
        {
            if (!PlayerExist(poolPlayers, playerId)) return;
            if (Vector2.SqrMagnitude((Vector2)  poolPlayers[playerId].transform.position - point) <
                UnityExtensions.ThresholdPosition)
            {
                 poolPlayers[playerId].StopMove();
            }
            else
            {
                 poolPlayers[playerId].SetMove(point);
            }
        }

        public void GameInfoReceived(JToken jToken)
        {
            foreach (var player in jToken.Children<JObject>())
            {
                var playerId = player[ServerParams.UserId].ToString();
                if (!poolPlayers.ContainsKey(playerId))
                    CreatePlayer(new Player(player[ServerParams.UserName].ToString(), playerId), false, true, prefabServerPlayer);
            }
        }

        public void GameTimeReceived(float time)
        {
            StartCoroutine(BattleGui.instance.mainGameTimer.UpdateTime((int)time));
        }

        public void GameResultReceived(JToken jToken)
        {
            var resultRound = BattleGui.instance.resultRound;
            resultRound.gameObject.SetActive(true);

            var lineIndex = 0;
            foreach (var player in jToken.Children<JObject>())
            {
                resultRound.Write(player[ServerParams.UserName].ToString(), resultRound.names, lineIndex);
                resultRound.Write(player[ServerParams.Kills].ToString(), resultRound.kills, lineIndex);
                resultRound.Write(player[ServerParams.Deaths].ToString(), resultRound.deaths, lineIndex);
                lineIndex++;
            }
            resultReceived = true;
        }

        public void LoginReceived(string playerId, string playerName)
        {
            CreatePlayer(new Player(playerName, playerId), false, true, prefabServerPlayer);
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

        /// <summary>
        /// check on uniqueness item with id(key) on map
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="pool"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        private bool RepeatKey<T>(ObjectPool<T> pool, string key) where T : MonoBehaviour
        {
            if (pool.ContainsKey(key))
            {
                Debug.LogWarning(string.Format("{0} at {1} already exists on map", key, pool[key].name));
                return true;
            }
            return false;
        }

        private bool PlayerExist(PlayerPool pool, string key)
        {
            if (pool.ContainsKey(key))
            {
                return true;
            }
            Debug.LogWarning("Player null, but received invoke");
            return false;
        }
    }
}
