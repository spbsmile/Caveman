using Caveman.Bonuses;
using Caveman.Configs;
using Caveman.Configs.Levels;
using Caveman.Level;
using Caveman.Network;
using Caveman.Players;
using Caveman.Pools;
using Caveman.DevSetting;
using Caveman.UI;
using Caveman.Weapons;
using UnityEngine;
using Random = System.Random;

namespace Caveman
{
    public class EnterPoint : MonoBehaviour
    {
        [SerializeField] private Transform prefabHero;
        [SerializeField] private Transform prefabBot;
        [SerializeField] private MapModel mapModel;
        [SerializeField] private PoolsManager poolsManager;
        [SerializeField] private string currentLevelName;
        [SerializeField] private LevelMode levelMode;
        [SerializeField] private SmoothCamera camera;
        [SerializeField] private PlayerPool playerPool;
        [SerializeField] private bool isObservableMode;

        protected SmoothCamera Camera => camera;
        protected PlayerPool PlayerPool => playerPool;
        protected bool IsObservableMode => isObservableMode;

        protected static MapConfig MapOfflineConfig { private set; get; }
        protected static string HeroId { private set; get; }
        protected IServerNotify serverNotify;
        protected PlayerManager PlayerManager;

        public static GameConfigs Configs { get; private set; }

        // caсhe fields for server message handler
        protected ObjectPool<WeaponModelBase> poolStones;
        protected ObjectPool<BonusBase> poolBonusesSpeed;
        protected BattleGui battleGui;

        protected Random rand;

        public void Awake()
        {
            Configs = GameConfigs.Load(
                "bonuses", "weapons", "players", "pools", "images", "maps", "levelsSingle", " ");
            HeroId = SystemInfo.deviceUniqueIdentifier;
            MapOfflineConfig = Configs.Map["sample"];
            rand = new Random();
        }

        public virtual void Start()
        {           
            CreateGui(false, isObservableMode, Configs.SingleLevel[currentLevelName].RoundTime);           

            CreatePoolManager(false);
            CreateCachePools();

            var mapConfig = Configs.Map["sample"];
            var mapCore = CreateMap(false, rand, mapConfig.Width, mapConfig.Heght, mapConfig);            
            CreatePlayersManager(rand, mapCore);
            CreateHero(Configs.Player["sample"]);
            CreateBots(Configs.SingleLevel[levelMode.ToString()], Configs.Player["sample"]);
            CreateCamera();
        }

        protected void CreatePoolManager(bool isMultiplayer)
        {
            poolsManager.InitializationPools(Configs, isMultiplayer);
        }

        protected void CreatePlayersManager(Random rand, MapCore mapCore)
        {
            PlayerManager = new PlayerManager(rand, playerPool, mapCore, poolsManager.ImagesDeath, levelMode);
        }

        //todo after get gameInfo if multiplayer 
        protected MapCore CreateMap(bool isMultiplayer, Random rand, int width, int height, MapConfig offlineConfig)
        {
            mapModel.InitializatonPool(poolsManager.Pools);
            return new MapCore(width, height, offlineConfig, isMultiplayer, mapModel, rand, levelMode);
        }

        protected void CreateCachePools()
        {
            poolStones = poolsManager.Stones;
            poolBonusesSpeed = poolsManager.BonusesSpeed;
        }

        //todo also after get gameInfo if multiplayer 
        // GameTimeReceive
        protected void CreateGui(bool isMultiplayer, bool isObservableMode, int roundTime)
        {
            battleGui = FindObjectOfType<BattleGui>();
            // todo change this after update code get time from server
            battleGui.Initialization(isMultiplayer, roundTime, isObservableMode, playerPool.GetCurrentPlayerModels, levelMode);
        }

        protected void CreateHero(PlayerConfig heroConfig)
        {
            PlayerManager.CreateHeroModel(
                    new PlayerCore(PlayerPrefs.GetString(AccountManager.KeyNickname),
                        HeroId, heroConfig),
                    Instantiate(prefabHero), battleGui.SubscribeOnEvents); 
        }

        private void CreateBots(SingleLevelConfig levelConfig, PlayerConfig botConfig)
        {
            for (var i = 1; i < levelConfig.BotsCount + 1; i++)
            {
                //todo name bots from bots config ??
                var playerCore = new PlayerCore(levelConfig.BotsName[i],
                    i.ToString(), botConfig);
                PlayerManager.CreateBotModel(playerCore, Instantiate(prefabBot), poolsManager.ContainerStones);
            }
        }

        // todo after create hero
        private void CreateCamera()
        {
            // todo miss typeing
            camera.Initialization(MapOfflineConfig.Width, MapOfflineConfig.Heght);
            camera.Watch(playerPool[HeroId].transform);
        }
    }
}
