using Caveman.Level;
using Caveman.Network;
using Caveman.Players;
using Caveman.Pools;
using Caveman.Setting;
using Caveman.UI;
using UnityEngine;
using Random = System.Random;

namespace Caveman
{
    public class EnterPoint : MonoBehaviour
    {
        public Transform prefabHumanPlayer;
        public Transform prefabAiPlayer;
        public MapModel mapModel;

        public string pathLevelSingleConfig;
        public string pathLevelMultiplayerConfig;
        public string currentLevelName;
        
        public SmoothCamera smoothCamera;

        protected IServerNotify serverNotify;
        protected PlayersManager playersManager;

        /// Get actual values from json
        public static CurrentGameSettings CurrentSettings { get; private set; }

        private Random rand;

        public void Awake()
        {
            // path: Resource/Settings
            CurrentSettings = CurrentGameSettings.Load(
                "bonuses", "weapons", "players", "pools", "images", "maps", pathLevelSingleConfig, pathLevelMultiplayerConfig);
        }

        public virtual void Start()
        {
            rand = new Random();
            var isMultiplayer = serverNotify != null;
            var mapCore = new MapCore(CurrentSettings.MapConfigs["sample"] , isMultiplayer, mapModel, rand);
            PoolsManager.instance.PrepareAllPools(CurrentSettings);

            var humanPlayer = new PlayerCore(PlayerPrefs.GetString(AccountManager.KeyNickname),
                SystemInfo.deviceUniqueIdentifier, CurrentSettings.PlayersConfigs["sample"]);
            BattleGui.instance.SubscribeOnEvents(humanPlayer);
            
            playersManager = new PlayersManager(serverNotify, smoothCamera, rand);

            playersManager.CreatePlayerModel(humanPlayer, false, false, Instantiate(prefabHumanPlayer));

            if (!isMultiplayer)
            {
                for (var i = 1; i < 2; i++)//CurrentSettings.LevelsSingleConfigs[currentLevelName].BotsCount + 1; i++)
                {
                    playersManager.CreatePlayerModel(
                        new PlayerCore("test"/*CurrentSettings.LevelsSingleConfigs[currentLevelName].BotsName[i]*/, i.ToString(),
	                        CurrentSettings.PlayersConfigs["sample"]),
                        true, false, prefabAiPlayer);
                }              
            }
        }
    }
}
