using Caveman.Players;
using Caveman.Pools;
using Caveman.UI;
using Caveman.Utils;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace Caveman.Network
{
    public class ServerMessageHandler : EnterPoint, IServerListener
    {
        public Transform prefabServerPlayer;

        //todo this const - temp. Defined on server 
        public const float WidthMapServer = 1350;
        public const float HeigthMapServer = 1350;
        private static string OwnId;
        private bool resultReceived;
	    private ServerConnection serverConnection;

        public override void Start()
        {
            OwnId = SystemInfo.deviceUniqueIdentifier;
            serverNotify = new ServerNotify {ServerListener = this};
	        serverConnection = (ServerConnection) serverNotify;
	        serverConnection.StartSession(OwnId,
                PlayerPrefs.GetString(AccountManager.KeyNickname));
            base.Start();
            serverNotify.RespawnSend(PlayerPool.instance[OwnId].transform.position);
        }

        public void Update()
        {
            if (!resultReceived) serverConnection.Update();
        }

        public void WeaponAddedReceive(string key, Vector2 point)
        {
            if (RepeatKey(poolStones, key)) return;
            poolStones.New(key).transform.position = point;
        }

        public void WeaponRemovedReceive(string key)
        {
            poolStones.Store(key);
        }

        public void WeaponPickReceive(string playerId, string key)
        {
            PlayerPool.instance[playerId].PickupWeapon(poolStones[key]);
        }

        public void WeaponUseReceive(string playerId, Vector2 aim)
        {
            PlayerPool.instance[playerId].ActivateWeapon(aim);
        }

        public void BonusAddedReceive(string key, Vector2 point)
        {
            if (RepeatKey(poolBonusesSpeed, key)) return;
            poolBonusesSpeed.New(key).transform.position = point;    
        }

        public void BonusRemovedReceive(string key, Vector2 point)
        {
            poolBonusesSpeed.Store(key);
        }

        public void BonusPickReceive(string playerId, string key)
        {
            PlayerPool.instance[playerId].PickupBonus(poolBonusesSpeed[key]);
        }

	    public void PlayerRespawnReceive(string playerId, Vector2 point)
	    {
		    PlayerPool.instance[playerId].RespawnInstantly(point);
	    }

	    public void PlayerDeadReceive(string playerId)
        {
            PlayerPool.instance[playerId].Die();
        }

        public void PlayerMoveReceive(string playerId, Vector2 point)
        {
            if (!PlayerExist(PlayerPool.instance, playerId)) return;
            if (Vector2.SqrMagnitude((Vector2)PlayerPool.instance[playerId].transform.position - point) <
                UnityExtensions.ThresholdPosition)
            {
                PlayerPool.instance[playerId].StopMove();
            }
            else
            {
                PlayerPool.instance[playerId].CalculateMoveUnit(point);
            }
        }

        public void GameInfoReceive(JToken jToken)
        {
            foreach (var player in jToken.Children<JObject>())
            {
                var playerId = player[ServerParams.UserId].ToString();
                if (!PlayerPool.instance.ContainsKey(playerId))
                    playersManager.CreatePlayerModel(new PlayerCore(player[ServerParams.UserName].ToString(), playerId,
		                    CurrentSettings.PlayersConfigs["sample"]), false, true, Instantiate(prefabServerPlayer));
            }
        }

        public void GameTimeReceive(float time)
        {
            StartCoroutine(BattleGui.instance.mainGameTimer.UpdateTime((int)time));
        }

        public void GameResultReceive(JToken jToken)
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

        public void LoginReceive(string playerId, string playerName)
        {
            playersManager.CreatePlayerModel(new PlayerCore(playerName, playerId,
	            CurrentSettings.PlayersConfigs["sample"]), false, true, Instantiate(prefabServerPlayer));
            serverNotify.RespawnSend(PlayerPool.instance[OwnId].transform.position);
        }

        public void LogoutReceive(string playerId)
        {
            PlayerPool.instance.Remove(playerId);
            Destroy(PlayerPool.instance[playerId].gameObject);
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
            Debug.LogWarning("PlayerCore null, but received invoke");
            return false;
        }
    }
}
