 using System.Collections;
using Caveman.Bonuses;
using Caveman.Level;
using Caveman.Network;
using Caveman.Players;
using Caveman.Pools;
using Caveman.Setting;
using Caveman.UI;
using Caveman.Utils;
using UnityEngine;
using Random = System.Random;
 using Caveman.Weapons;

namespace Caveman
{
    public class EnterPoint : MonoBehaviour
    {
        public Transform prefabHumanPlayer;
        public Transform prefabAiPlayer;

        public string pathLevelSingleConfig;
        public string pathLevelMultiplayerConfig;
        public string currentLevelName;
        
        public SmoothCamera smoothCamera;

        protected IClientListener serverNotify;
        protected PlayersManager playersManager;

        /// Get actual values from json
        public static CurrentGameSettings CurrentSettings { get; private set; }

        private Random r;

        public virtual void Awake()
        {
            // path: Resource/Settings
            CurrentSettings = CurrentGameSettings.Load(
                "bonuses", "weapons", "players", "pools", "images", pathLevelSingleConfig, pathLevelMultiplayerConfig);

            //todo
            //  LoadingScreen.instance.FinishLoading += (o, s) => Play();
        }

        public virtual void Start()
        {
            r = new Random();
            PoolsManager.instance.PrepareAllPools(CurrentSettings);

            var humanPlayer = new PlayerCore(PlayerPrefs.GetString(AccountManager.KeyNickname),
                SystemInfo.deviceUniqueIdentifier);
            BattleGui.instance.SubscribeOnEvents(humanPlayer);
            
            playersManager = new PlayersManager(serverNotify, smoothCamera, r);
            var mapManager = new MapManager(serverNotify);

            playersManager.CreatePlayerModel(humanPlayer, false, false, Instantiate(prefabHumanPlayer));


            if (serverNotify == null)
            {
                for (var i = 1; i < CurrentSettings.LevelsSingleConfigs[currentLevelName].BotsCount + 1; i++)
                {
                    playersManager.CreatePlayerModel(
                        new PlayerCore(CurrentSettings.LevelsSingleConfigs[currentLevelName].BotsName[i], i.ToString()),
                        true, false, prefabAiPlayer);
                }
                PutAllItemsOnMap(new[] {"weapons", "bonuses"});
            }
        }

        // todo virtual &
      /*  public virtual void Play()
        {
            //  foreach (var player in poolPlayers.GetCurrentPlayers())
            //  {
            //      player.Play();
            //  }
        }
        */
        private void PutAllItemsOnMap(string[] typesItems)
        {
            for (var i = 0; i < typesItems.Length; i++)
            {
                switch (typesItems[i])
                {
                    case "weapons":
                        // todo sort config to folder/levels game
                        foreach (var weaponConfig in CurrentSettings.WeaponsConfigs.Values)
                        {
                            StartCoroutine(PutItemsOnMap<WeaponModelBase>(weaponConfig.PrefabPath,
                                weaponConfig.TimeRespawn));
                        }
                        break;
                    case "bonuses":
                        foreach (var bonusConfig in CurrentSettings.BonusesConfigs.Values)
                        {
                            StartCoroutine(PutItemsOnMap<BonusBase>(bonusConfig.PrefabPath, bonusConfig.TimeRespawn));
                        }
                        break;
                }
            }
        }

        private IEnumerator PutItemsOnMap<T>(string poolId, float timeRespawn) where T : MonoBehaviour
        {
            yield return new WaitForSeconds(timeRespawn);
            //todo length
            // todo var bound = Settings.BonusSpeedMaxCount - PoolsManager.instance.BonusesSpeed.GetActivedCount; 
            for (var i = 0; i < Settings.WeaponInitialLying; i++)
            {
                PutItemOnMap((ObjectPool<T>) PoolsManager.instance.Pools[poolId]);
            }
            StartCoroutine(PutItemsOnMap<T>(poolId, timeRespawn));
        }

        private void PutItemOnMap<T>(ObjectPool<T> pool) where T : MonoBehaviour
        {
            var item = pool.New();
            StartCoroutine(UnityExtensions.FadeIn(item.GetComponent<SpriteRenderer>()));
            item.transform.position = new Vector2(r.Next(1, Settings.WidthMap - 1), r.Next(1, Settings.HeightMap - 1));
        }
    }
}
