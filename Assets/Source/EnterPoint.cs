using System;
using System.Collections;
using Caveman.CustomAnimation;
using Caveman.Bonuses;
using Caveman.Level;
using Caveman.Network;
using Caveman.Players;
using Caveman.Pools;
using Caveman.Setting;
using Caveman.Specification;
using Caveman.UI;
using Caveman.Utils;
using Caveman.Weapons;
using UnityEngine;
using Random = System.Random;
using Caveman.UI.Common;

namespace Caveman
{
    public class EnterPoint : MonoBehaviour
    {
        public Transform prefabHumanPlayer;
        public Transform prefabAiPlayer;

        public AxeModel prefabAxe;
        public StoneModel prefabStone;
        public StoneSplash prefabStoneFlagmentInc;
        public EffectBase prefabDeathImage;
        public SpeedBonus prefabBonusSpeed;

        public SmoothCamera smoothCamera;
        public Transform containerStones;
        public Transform containerSplashStones;
        public Transform containerSkulls;
        public Transform containerDeathImages;
        public Transform containerPlayers;
        public Transform containerBonusesSpeed;

        protected Random r;
        protected IClientListener serverNotify;
        protected PlayerPool poolPlayers;
        protected ObjectPool<WeaponModelBase> poolStones;
        protected ObjectPool<WeaponModelBase> poolSkulls;
        protected ObjectPool<BonusBase> poolBonusesSpeed;
        protected ObjectPool<EffectBase> poolStonesSplash;
        protected ObjectPool<EffectBase> poolDeathImage;
        
        //used only single player mode for bots
        //todo deleted this. get this data from json
        private readonly string[] names = { "Kiracosyan", "IkillU", "skaska", "loser", "yohoho", "shpuntik" };

        /// <summary>
        /// Get actual value of something parameter specification  
        /// </summary>
        public static CurrentGameSettings CurrentSettings { get; private set; }

        public virtual void Awake()
        {
            // load data from json files
            CurrentSettings = CurrentGameSettings.Load();
            //todo strange usage. settings type - deprecate
            Settings.multiplayerMode = false;
            //todo
            LoadingScreen.instance.FinishLoading += (o, s) => Play();
        }

        public virtual void Start()
        {
            //todo may be use only UnityEngine.Random
            r = new Random();

            poolStonesSplash = PreparePool<EffectBase>(Settings.PoolCountSplashStones, containerSplashStones, prefabStoneFlagmentInc, null);
            poolDeathImage = PreparePool<EffectBase>(Settings.PoolCountDeathImages, containerDeathImages, prefabDeathImage, null);
            poolStones = PreparePool<WeaponModelBase>(Settings.PoolCountStones, containerStones, prefabStone, InitStoneModel);
            poolSkulls = PreparePool<WeaponModelBase>(Settings.PoolCountSkulls, containerSkulls, prefabAxe, InitSkullModel);
            poolBonusesSpeed = PreparePool<BonusBase>(Settings.BonusSpeedPoolCount, containerBonusesSpeed, prefabBonusSpeed, InitBonusModel);

            poolStones.RelatedPool += () => poolStonesSplash;

            poolPlayers = containerPlayers.GetComponent<PlayerPool>();

            var humanPlayer = new Player(PlayerPrefs.GetString(AccountManager.KeyNickname),
                SystemInfo.deviceUniqueIdentifier);
            BattleGui.instance.SubscribeOnEvents(humanPlayer);
            BattleGui.instance.resultRound.SetPlayerPool(poolPlayers);
            BattleGui.instance.waitForResp.SetPlayerPool(poolPlayers);
            CreatePlayer(humanPlayer, false, false, prefabHumanPlayer);
            
            if (serverNotify == null)
            {
                for (var i = 1; i < Settings.BotsCount + 1; i++)
                {
                    CreatePlayer(new Player(names[i], i.ToString()), true, false, prefabAiPlayer);
                }
                StartCoroutine(PutWeaponsOnMap());
                StartCoroutine(PutBonusesOnMap());
            }
        }

        public virtual void Play()
        {
            foreach (var player in poolPlayers.GetCurrentPlayers())
            {
                player.Play();
            }
        }
        
        private IEnumerator PutBonusesOnMap()
        {
            var bound = Settings.BonusSpeedMaxCount - poolBonusesSpeed.GetActivedCount; 
            for (var i = 0; i < bound; i++)
            {
                PutItemOnMap(poolBonusesSpeed);
            }
            yield return new WaitForSeconds(Settings.BonusTimeRespawn);
            StartCoroutine(PutBonusesOnMap());
        }

        private IEnumerator PutWeaponsOnMap()
        {
            for (var i = 0; i < Settings.WeaponInitialLying; i++)
            {
                PutItemOnMap(poolStones);
            }
            for (var i = 0; i < Settings.CountLyingSkulls; i++)
            {
                PutItemOnMap(poolSkulls);
            }
            yield return new WaitForSeconds(Settings.WeaponTimeRespawn);
            StartCoroutine(PutWeaponsOnMap());
        }

        private void PutItemOnMap<T>(ObjectPool<T> pool) where T : MonoBehaviour
        {
            var item = pool.New();
            StartCoroutine(UnityExtensions.FadeIn(item.GetComponent<SpriteRenderer>()));
            item.transform.position = new Vector2(r.Next(1, Settings.WidthMap - 1), r.Next(1, Settings.HeightMap - 1));
        }

        protected void CreatePlayer(Player player, bool isAiPlayer, bool isServerPlayer, Transform prefabModel)
        {
            var prefab = Instantiate(prefabModel);
            var playerModel = prefab.GetComponent<PlayerModelBase>();
            if (!isServerPlayer)
            {
                if (isAiPlayer)
                {
                    (playerModel as AiPlayerModel).SetWeapons(containerStones);
                }
                else
                {
                    BattleGui.instance.SubscribeOnEvents(playerModel);
                    smoothCamera.target = prefab.transform;
                    smoothCamera.SetPlayer(prefab.GetComponent<PlayerModelBase>());
                    if (serverNotify != null) playerModel.GetComponent<SpriteRenderer>().material.color = Color.red;
                }
            }
            playerModel.Init(player, r, poolPlayers, serverNotify);
            poolPlayers.Add(player.Id, playerModel);
            playerModel.transform.SetParent(containerPlayers);
            playerModel.Death += position => StartCoroutine(DeathAnimate(position));
            playerModel.ChangedWeaponsPool += SwitchPoolWeapons;
            playerModel.Birth(new Vector2(r.Next(1, Settings.WidthMap - 1), r.Next(1, Settings.HeightMap - 1)));
        }      

        // todo extracted this method from enterpoint
        /// <summary>
        /// used player
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        private IEnumerator DeathAnimate(Vector2 position)
        {
            var deathImage = poolDeathImage.New();
            deathImage.transform.position = position;
            var spriteRenderer = deathImage.GetComponent<SpriteRenderer>();
            if (spriteRenderer)
            {
                for (var i = 1f; i > 0; i -= 0.1f)
                {
                    var c = spriteRenderer.color;
                    c.a = i;
                    spriteRenderer.color = c;
                    yield return new WaitForSeconds(0.1f);
                }
            }
            poolDeathImage.Store(deathImage);
        }
    }
}
