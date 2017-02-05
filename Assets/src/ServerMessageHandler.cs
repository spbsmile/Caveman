using System.Linq;
using Caveman.Configs;
using Caveman.Players;
using Caveman.Pools;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace Caveman.Network
{
    public class ServerMessageHandler : EnterPoint, IServerListener
    {        
        public Transform prefabServerPlayer;        

        public static MapConfig MapServerConfig { private set; get;}
        
        private bool resultReceived;
        private ServerConnection serverConnection;

        public override void Start()
        {            
            serverNotify = new ServerNotify {ServerListener = this};
            serverConnection = (ServerConnection) serverNotify;
            serverConnection.StartSession(HeroId,
                PlayerPrefs.GetString(AccountManager.KeyNickname), isObservableMode);
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
            playerPool[playerId].PickupWeapon(poolStones[key]);
        }

        public void WeaponUseReceive(string playerId, Vector2 aim)
        {
            playerPool[playerId].ActivateWeapon(aim);
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
            playerPool[playerId].PickupBonus(poolBonusesSpeed[key]);
        }

        public void PlayerRespawnReceive(string playerId, Vector2 point)
        {
            playerPool[playerId].RespawnInstantly(point);
        }

        public void PlayerDeadReceive(string playerId)
        {
            playerPool[playerId].Die();
        }

        public void PlayerMoveReceive(string playerId, Vector2 targetPoint)
        {
            if (!PlayerExist(playerPool, playerId)) return;
            playerPool[playerId].CalculateMoveUnit(targetPoint);
        }

        /*  todo use map config
            http://stackoverflow.com/questions/11927135/converting-a-jtoken-or-string-to-a-given-type
        */
        public void GameInfoMapReceive(JObject jObject)
        {
            var mapConfig = JsonConvert.DeserializeObject<MapConfig>(jObject.ToString());
            MapServerConfig = mapConfig;            
            var mapCore = CreateMap(true, rand, mapConfig.Width, mapConfig.Heght, MapConfig);
            CreatePlayersManager(rand, mapCore);

            //todo after get round time from server
            CreateGui(true, isObservableMode, 0);     
            CreateHero(Configs.Player["sample"]);              
        }

        public void GameInfoPlayersReceive(JToken jToken)
        {
            foreach (var player in jToken.Children<JObject>())
            {
                var playerId = player[ServerParams.UserId].ToString();
                if (!playerPool.ContainsKey(playerId))
                    playersManager.CreateServerModel(new PlayerCore(player[ServerParams.UserName].ToString(), playerId,
                        Configs.Player["sample"]), Instantiate(prefabServerPlayer));
            }            
            if (!camera.IsWatcher && playerPool.GetCurrentPlayerModels().Any())
            {                 
                camera.Watch(playerPool.GetCurrentPlayerModels().First().transform);
            }
        }        

        public void GameTimeReceive(float time)
        {               
            StartCoroutine(battleGui.mainGameTimer.UpdateTime((int) time));
        }

        public void GameResultReceive(JToken jToken)
        {
            var resultRound = battleGui.resultRound;
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
            playersManager.CreateServerModel(new PlayerCore(playerName, playerId,
                Configs.Player["sample"]), Instantiate(prefabServerPlayer));
            serverNotify.RespawnSend(playerPool[HeroId].transform.position);
            if (!camera.IsWatcher)
            {
                camera.Watch(playerPool[playerId].transform);
            }
        }

        public void LogoutReceive(string playerId)
        {
            playerPool.Remove(playerId);
            Destroy(playerPool[playerId].gameObject);
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