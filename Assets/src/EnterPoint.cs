using Caveman.Bonuses;
using Caveman.Configs;
using Caveman.Configs.Levels;
using Caveman.Level;
using Caveman.Network;
using Caveman.Players;
using Caveman.Pools;
using Caveman.Setting;
using Caveman.UI;
using Caveman.Weapons;
using UnityEngine;
using Random = System.Random;

namespace Caveman
{
    public class EnterPoint : MonoBehaviour
    {
        // this fields initialization from scene
        public Transform prefabHero;
        public Transform prefabBot;
        public MapModel mapModel;
        public PoolsManager poolsManager;
        public SmoothCamera camera;
        public PlayerPool playerPool;
        public string currentLevelName;
        public static GameConfigs Configs { get; private set; }
        public static MapConfig MapConfig { set; get; }

        public static string HeroId { private set; get; }

        public bool isObservableMode;

        protected IServerNotify serverNotify;
        protected PlayersManager playersManager;

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
            MapConfig = Configs.Map["sample"];
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
            CreateBots(Configs.SingleLevel[currentLevelName], Configs.Player["sample"]);
            CreateCamera();
        }

        public void CreatePoolManager(bool isMultiplayer)
        {
            poolsManager.InitializationPools(Configs, isMultiplayer);
        }

        public void CreatePlayersManager(Random rand, MapCore mapCore)
        {
            playersManager = new PlayersManager(rand, playerPool, mapCore, poolsManager.ChangeWeaponPool, poolsManager.ImagesDeath);
        }

        //todo after get gameInfo if multiplayer 
        public MapCore CreateMap(bool isMultiplayer, Random rand, int width, int height, MapConfig offlineConfig)
        {
            mapModel.InitializatonPool(poolsManager.Pools);
            return new MapCore(width, height, offlineConfig, isMultiplayer, mapModel, rand);
        }

        public void CreateCachePools()
        {
            poolStones = poolsManager.Stones;
            poolBonusesSpeed = poolsManager.BonusesSpeed;
        }

        //todo also after get gameInfo if multiplayer 
        // GameTimeReceive
        public void CreateGui(bool isMultiplayer, bool isObservableMode, int roundTime)
        {
            battleGui = FindObjectOfType<BattleGui>();
            // todo change this after update code get time from server
            battleGui.Initialization(isMultiplayer, roundTime, isObservableMode, playerPool.GetCurrentPlayerModels);
        }        

        public void CreateHero(PlayerConfig heroConfig)
        {
            playersManager.CreateHeroModel(
                    new PlayerCore(PlayerPrefs.GetString(AccountManager.KeyNickname),
                        HeroId, heroConfig),
                    Instantiate(prefabHero), battleGui.SubscribeOnEvents);
        }

        public void CreateBots(SingleLevelConfig levelConfig, PlayerConfig botConfig)
        {
            for (var i = 1; i < levelConfig.BotsCount + 1; i++)
            {
                //todo name bots from bots config ??
                var playerCore = new PlayerCore(levelConfig.BotsName[i],
                    i.ToString(), botConfig);
                playersManager.CreateBotModel(playerCore, Instantiate(prefabBot), poolsManager.containerStones);
            }
        }

        // todo after create hero
        public void CreateCamera()
        {
            // todo miss typeing
            camera.Initialization(MapConfig.Width, MapConfig.Heght);
            camera.Watch(playerPool[HeroId].transform);
        }
    }
}
