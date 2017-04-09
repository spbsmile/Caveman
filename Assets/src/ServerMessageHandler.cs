using System.Linq;
using Caveman.Configs;
using Caveman.Players;
using Caveman.Pools;
using Caveman.Weapons;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace Caveman.Network
{
    public class ServerMessageHandler : EnterPoint, IServerListener
    {        
        [SerializeField] private Transform prefabServerPlayer;

        public static MapConfig MapServerConfig { private set; get;}
        
        private bool resultReceived;
        private ServerConnection serverConnection;

        public override void Start()
        {            
            serverNotify = new ServerNotify {ServerListener = this};
            serverConnection = (ServerConnection) serverNotify;
            serverConnection.StartSession(HeroId,
                PlayerPrefs.GetString(AccountManager.KeyNickname), IsObservableMode);
            CreatePoolManager(true);
            CreateCachePools();                     
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
            PlayerPool[playerId].PickupWeapon((IWeapon)poolStones[key]);
        }

        public void WeaponUseReceive(string playerId, Vector2 aim)
        {
            PlayerPool[playerId].ActivateWeapon(aim);
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
            PlayerPool[playerId].PickupBonus(poolBonusesSpeed[key]);
        }

        public void PlayerRespawnReceive(string playerId, Vector2 point)
        {
            PlayerPool[playerId].RespawnInstantly(point);
        }

        public void PlayerDeadReceive(string playerId)
        {
            PlayerPool[playerId].Die();
        }

        public void PlayerMoveReceive(string playerId, Vector2 targetPoint)
        {
            if (!PlayerExist(PlayerPool, playerId)) return;
            PlayerPool[playerId].CalculateMoveUnit(targetPoint);
        }
        
        public void GameInfoMapReceive(JObject jObject)
        {
            MapServerConfig = JsonConvert.DeserializeObject<MapConfig>(jObject.ToString());                    
            var mapCore = CreateMap(true, rand, MapServerConfig.Width, MapServerConfig.Heght, MapOfflineConfig);
            CreatePlayersManager(rand, mapCore);

            //todo after get round time from server
            CreateGui(true, IsObservableMode, 0);
            if(!IsObservableMode)
            {
                CreateHero(Configs.Player["sample"]);              
            }                        
        }

        public void GameInfoPlayersReceive(JToken jToken)
        {
            foreach (var player in jToken.Children<JObject>())
            {
                var playerId = player[ServerParams.UserId].ToString();
                if (!PlayerPool.ContainsKey(playerId))
                    PlayerManager.CreateServerModel(new PlayerCore(player[ServerParams.UserName].ToString(), playerId,
                        Configs.Player["sample"]), Instantiate(prefabServerPlayer));
            }            
            if (!Camera.IsWatcher && PlayerPool.GetCurrentPlayerModels().Any())
            {                 
                Camera.Watch(PlayerPool.GetCurrentPlayerModels().First().transform);
            }
        }        

        public void GameTimeReceive(float time)
        {               
            StartCoroutine(battleGui.MainGameTimer.UpdateTime((int) time));
        }

        public void GameResultReceive(JToken jToken)
        {
            var resultRound = battleGui.ResultRound;
            resultRound.gameObject.SetActive(true);

            var lineIndex = 0;
            foreach (var player in jToken.Children<JObject>())
            {
                resultRound.Write(player[ServerParams.UserName].ToString(), resultRound.Names, lineIndex);
                resultRound.Write(player[ServerParams.Kills].ToString(), resultRound.Kills, lineIndex);
                resultRound.Write(player[ServerParams.Deaths].ToString(), resultRound.Deaths, lineIndex);
                lineIndex++;
            }
            resultReceived = true;
        }

        public void LoginReceive(string playerId, string playerName)
        {            
            PlayerManager.CreateServerModel(new PlayerCore(playerName, playerId,
                Configs.Player["sample"]), Instantiate(prefabServerPlayer));
            serverNotify.RespawnSend(PlayerPool[HeroId].transform.position);
            if (!Camera.IsWatcher)
            {
                Camera.Watch(PlayerPool[playerId].transform);
            }
        }

        public void LogoutReceive(string playerId)
        {
            PlayerPool.Remove(playerId);
            Destroy(PlayerPool[playerId].gameObject);
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
                Debug.LogWarning($"{key} at {pool[key].name} already exists on map");
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