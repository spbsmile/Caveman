using System;
using System.Collections;
using Caveman.CustomAnimation;
using Caveman.Bonuses;
using Caveman.Level;
using Caveman.Network;
using Caveman.Players;
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
        private readonly string[] names = { "Kiracosyan", "IkillU", "skaska", "loser", "yohoho", "shpuntik" };

        public static CurrentGameSettings CurrentSettings { get; private set; }

        public virtual void Awake()
        {
            // load data from json files
            CurrentSettings = CurrentGameSettings.Load();
            Settings.multiplayerMode = false;
            LoadingScreen.instance.FinishLoading += new EventHandler((o, s) => Play());
        }

        public virtual void Start()
        {
            //todo may be use only UnityEngine.Random
            r = new Random();

            poolStonesSplash = CreatePool<EffectBase>(Settings.PoolCountSplashStones, containerSplashStones, prefabStoneFlagmentInc, null);
            poolDeathImage = CreatePool<EffectBase>(Settings.PoolCountDeathImages, containerDeathImages, prefabDeathImage, null);
            poolStones = CreatePool<WeaponModelBase>(Settings.PoolCountStones, containerStones, prefabStone, InitStoneModel);
            poolSkulls = CreatePool<WeaponModelBase>(Settings.PoolCountSkulls, containerSkulls, prefabAxe, InitSkullModel);
            poolBonusesSpeed = CreatePool<BonusBase>(Settings.BonusSpeedPoolCount, containerBonusesSpeed, prefabBonusSpeed, InitBonusModel);

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
                StartCoroutine(PutWeapons());
                StartCoroutine(PutBonuses());
            }
        }

        public virtual void Play()
        {
            var players = poolPlayers.GetCurrentPlayers();
            foreach (PlayerModelBase player in players)
                player.Play();
        }
        /// <summary>
        /// Each model assigned reference on objectpool
        /// </summary>
        /// <param name="item"></param>
        /// <param name="pool"></param>
        private void InitBonusModel(GameObject item, ObjectPool<BonusBase> pool)
        {
            item.GetComponent<BonusBase>().SetPool(pool);
        }

        private void InitSkullModel(GameObject item, ObjectPool<WeaponModelBase> pool) 
        {
            item.GetComponent<AxeModel>().SetPool(pool);
        }

        private void InitStoneModel(GameObject item, ObjectPool<WeaponModelBase> pool)
        {
            var model = item.GetComponent<StoneModel>();
            model.SetPool(pool);
            model.SetPoolSplash(poolStonesSplash);
        }

        private IEnumerator PutBonuses()
        {
            var bound = Settings.BonusSpeedMaxCount - poolBonusesSpeed.GetActivedCount; 
            for (var i = 0; i < bound; i++)
            {
                PutItem(poolBonusesSpeed);
            }
            yield return new WaitForSeconds(Settings.BonusTimeRespawn);
            StartCoroutine(PutBonuses());
        }

        private IEnumerator PutWeapons()
        {
            for (var i = 0; i < Settings.WeaponInitialLying; i++)
            {
                PutItem(poolStones);
            }
            for (var i = 0; i < Settings.CountLyingSkulls; i++)
            {
                PutItem(poolSkulls);
            }
            yield return new WaitForSeconds(Settings.WeaponTimeRespawn);
            StartCoroutine(PutWeapons());
        }

        private void PutItem<T>(ObjectPool<T> pool) where T : MonoBehaviour
        {
            var item = pool.New();
            var sprite = item.GetComponent<SpriteRenderer>();
            StartCoroutine(UnityExtensions.FadeIn(sprite));

            // put item in random position
            var x = r.Next(1, Settings.WidthMap - 1);
            var y = r.Next(1, Settings.HeightMap - 1);
            item.transform.position = new Vector2(x, y);
        }

        /// <summary>
        /// Used object pool pattern
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="initialBufferSize"></param>
        /// <param name="container"></param>
        /// <param name="prefab"></param>
        /// <param name="init"></param>
        /// <returns></returns>
        private ObjectPool<T> CreatePool<T>(int initialBufferSize, Transform container, T prefab, Action<GameObject, ObjectPool<T>> init) where T : MonoBehaviour
        {
            var pool = container.GetComponent<ObjectPool<T>>();
            pool.CreatePool(prefab, initialBufferSize, serverNotify != null);
            for (var i = 0; i < initialBufferSize; i++)
            {
                var item = Instantiate(prefab);
                if (init != null)
                {
                    init(item.gameObject, pool);
                }
                item.transform.SetParent(container);
                pool.Store(item);
            }
            return pool;
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
            playerModel.ChangedWeaponsPool += ChangedWeapons;
            playerModel.Birth(new Vector2(r.Next(1, Settings.WidthMap - 1), r.Next(1, Settings.HeightMap - 1)));
        }

        /// <summary>
        /// Changed weapon pool storage in player 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private ObjectPool<WeaponModelBase> ChangedWeapons(WeaponSpecification.Types type)
        {
            switch (type)
            {
                case WeaponSpecification.Types.Stone:
                    return poolStones;
                case WeaponSpecification.Types.Skull:
                    return poolSkulls;
            }
            return null;
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
