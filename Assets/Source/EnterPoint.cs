using Caveman.Bonuses;
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
        public Transform prefabHumanPlayer;
        public Transform prefabAiPlayer;
        public MapModel mapModel;
        public PoolsManager poolsManager;
        public SmoothCamera smoothCamera;
        public PlayerPool playerPool;
        public string currentLevelName;

        public bool isObservableMode;

        public static CurrentGameSettings CurrentSettings { get; private set; }

        protected IServerNotify serverNotify;
        protected PlayersManager playersManager;

        // caсhe fields for server message handler
        protected ObjectPool<WeaponModelBase> poolStones;
        protected ObjectPool<BonusBase> poolBonusesSpeed;
        protected BattleGui battleGui;


        public void Awake()
        {
            CurrentSettings = CurrentGameSettings.Load(
                "bonuses", "weapons", "players", "pools", "images", "maps", "levelsSingle", " ");
        }

        public virtual void Start()
        {
            var isMultiplayer = serverNotify != null;
            battleGui = FindObjectOfType<BattleGui>();
            battleGui.Initialization(isMultiplayer, CurrentSettings.SingleLevelConfigs[currentLevelName].RoundTime, isObservableMode, playerPool.GetCurrentPlayerModels);

            var rand = new Random();
          
            poolsManager.InitializationPools(CurrentSettings, isMultiplayer);
            poolStones = poolsManager.Stones;
            poolBonusesSpeed = poolsManager.BonusesSpeed;

            mapModel.InitializatonPool(poolsManager.Pools);
            var mapCore = new MapCore(CurrentSettings.MapConfigs["sample"] , isMultiplayer, mapModel, rand);

            smoothCamera.Initialization(mapCore.Width, mapCore.Height);
            playersManager = new PlayersManager(serverNotify, smoothCamera, rand, playerPool, mapCore, poolsManager.ChangeWeaponPool, poolsManager.ImagesDeath);

            if (!isObservableMode)
            {
                playersManager.CreatePlayerModel(
                    new PlayerCore(PlayerPrefs.GetString(AccountManager.KeyNickname),
                        SystemInfo.deviceUniqueIdentifier, CurrentSettings.PlayersConfigs["sample"]),
                    Instantiate(prefabHumanPlayer), battleGui);
            }

            if (!isMultiplayer)
            {
                for (var i = 1; i < CurrentSettings.SingleLevelConfigs[currentLevelName].BotsCount + 1; i++)
                {
                    var playerCore = new PlayerCore(CurrentSettings.SingleLevelConfigs[currentLevelName].BotsName[i],
                        i.ToString(),
                        CurrentSettings.PlayersConfigs["sample"]);
                    playersManager.CreateClientAiModel(playerCore, Instantiate(prefabAiPlayer), poolsManager.containerStones);
                }
                playersManager.StartUseWeapon();
            }
        }
    }
}
