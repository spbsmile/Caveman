using Caveman.Players;
using Caveman.Pools;
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

        public override void Start()
        {
            OwnId = SystemInfo.deviceUniqueIdentifier;
            serverNotify = new ServerNotify {ServerListener = this};
            serverNotify.StartSession(OwnId,
                PlayerPrefs.GetString(AccountManager.KeyNickname));
            base.Start();
            serverNotify.Respawn(PlayerPool.instance[OwnId].transform.position);
        }

        public override void Play()
        {
            base.Play();
            serverNotify.Respawn(PlayerPool.instance[OwnId].transform.position);
        }

        public void Update()
        {
            if (!resultReceived) serverNotify.Update();
        }

        public void WeaponAddedReceived(string key, Vector2 point)
        {
            if (RepeatKey(PoolsManager.instance.Stones, key)) return;
            PoolsManager.instance.Stones.New(key).transform.position = point;
        }

        public void WeaponRemovedReceived(string key)
        {
            PoolsManager.instance.Stones.Store(key);
        }

        public void WeaponPickReceived(string playerId, string key)
        {
            PlayerPool.instance[playerId].PickupWeapon(PoolsManager.instance.Stones[key]);
        }

        public void WeaponUseReceived(string playerId, Vector2 aim)
        {
            PlayerPool.instance[playerId].ThrowWeapon(aim);
        }

        public void BonusAddedReceived(string key, Vector2 point)
        {
            if (RepeatKey(PoolsManager.instance.BonusesSpeed, key)) return;
            PoolsManager.instance.BonusesSpeed.New(key).transform.position = point;    
        }

        public void BonusRemovedReceived(string key, Vector2 point)
        {
            PoolsManager.instance.BonusesSpeed.Store(key);
        }

        public void BonusPickReceived(string playerId, string key)
        {
            PlayerPool.instance[playerId].PickupBonus(PoolsManager.instance.BonusesSpeed[key]);
        }

        public void PlayerRespawnReceived(string playerId, Vector2 point)
        {
            if (PlayerPool.instance[playerId].firstRespawn)
            {
                PlayerPool.instance[playerId].firstRespawn = false;
                PlayerPool.instance[playerId].transform.position = point;
            }
            else
            {
                PlayerPool.instance[playerId].Birth(point);
            }
        }

        public void PlayerDeadReceived(string playerId)
        {
            PlayerPool.instance[playerId].Die();
        }

        public void PlayerMoveReceived(string playerId, Vector2 point)
        {
            if (!PlayerExist(PlayerPool.instance, playerId)) return;
            if (Vector2.SqrMagnitude((Vector2)PlayerPool.instance[playerId].transform.position - point) <
                UnityExtensions.ThresholdPosition)
            {
                PlayerPool.instance[playerId].StopMove();
            }
            else
            {
                PlayerPool.instance[playerId].SetMove(point);
            }
        }

        public void GameInfoReceived(JToken jToken)
        {
            foreach (var player in jToken.Children<JObject>())
            {
                var playerId = player[ServerParams.UserId].ToString();
                if (!PlayerPool.instance.ContainsKey(playerId))
                    CreatePlayerModel(new Player(player[ServerParams.UserName].ToString(), playerId), false, true, prefabServerPlayer);
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
            CreatePlayerModel(new Player(playerName, playerId), false, true, prefabServerPlayer);
            serverNotify.Respawn(PlayerPool.instance[OwnId].transform.position);
        }

        public void LogoutReceived(string playerId)
        {
            PlayerPool.instance.Remove(playerId);
            Destroy(PlayerPool.instance[playerId].gameObject);
        }

        public void OnDestroy()
        {
            serverNotify.StopSession();
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
