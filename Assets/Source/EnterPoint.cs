using Caveman.Bonuses;
using Caveman.CustomAnimation;
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
        public Transform prefabHumanPlayer;
        public Transform prefabAiPlayer;
        public MapModel mapModel;

        public string currentLevelName;

        public PoolsManager poolsManager;
        public SmoothCamera smoothCamera;

        protected IServerNotify serverNotify;
        protected PlayersManager playersManager;

        public static CurrentGameSettings CurrentSettings { get; private set; }

        private Random rand;

        public void Awake()
        {
            CurrentSettings = CurrentGameSettings.Load(
                "bonuses", "weapons", "players", "pools", "images", "maps", "levelsSingle", " ");
        }

        public virtual void Start()
        {
            rand = new Random();
            var isMultiplayer = serverNotify != null;

            PoolsManager.instance.PrepareAllPools(CurrentSettings);
            new MapCore(CurrentSettings.MapConfigs["sample"] , isMultiplayer, mapModel, rand);

            var humanCore = new PlayerCore(PlayerPrefs.GetString(AccountManager.KeyNickname),
                SystemInfo.deviceUniqueIdentifier, CurrentSettings.PlayersConfigs["sample"]);
            BattleGui.instance.SubscribeOnEvents(humanCore);
            playersManager = new PlayersManager(serverNotify, smoothCamera, rand);
            playersManager.CreatePlayerModel(humanCore, false, false, Instantiate(prefabHumanPlayer));

            if (!isMultiplayer)
            {
                for (var i = 1; i < CurrentSettings.SingleLevelConfigs[currentLevelName].BotsCount + 1; i++)
                {
                    var playerCore = new PlayerCore(CurrentSettings.SingleLevelConfigs[currentLevelName].BotsName[i],
                        i.ToString(),
                        CurrentSettings.PlayersConfigs["sample"]);

                    playersManager.CreatePlayerModel(playerCore,
                        true, false, Instantiate(prefabAiPlayer));
                }
            }
        }
    }
}
