﻿using System.Collections;
using Caveman.Level;
using Caveman.Network;
using Caveman.Players;
using Caveman.Pools;
using Caveman.Setting;
using Caveman.UI;
using Caveman.Utils;
using UnityEngine;
using Random = System.Random;
using Caveman.UI.Common;

namespace Caveman
{
    public class EnterPoint : MonoBehaviour
    {
        public Transform prefabHumanPlayer;
        public Transform prefabAiPlayer;

        public SmoothCamera smoothCamera;

        private Random r;
        protected IClientListener serverNotify;
      
        
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
            r = new Random();
            
            PoolManager.instance.Stones.RelatedPool += () => PoolManager.instance.SplashesStone;
            var humanPlayer = new Player(PlayerPrefs.GetString(AccountManager.KeyNickname),
                SystemInfo.deviceUniqueIdentifier);
            BattleGui.instance.SubscribeOnEvents(humanPlayer);
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
         //   foreach (var player in poolPlayers.GetCurrentPlayers())
          //  {
          //      player.Play();
          //  }
        }
        
        private IEnumerator PutBonusesOnMap()
        {
            var bound = Settings.BonusSpeedMaxCount - PoolManager.instance.BonusesSpeed.GetActivedCount; 
            for (var i = 0; i < bound; i++)
            {
                PutItemOnMap(PoolManager.instance.BonusesSpeed);
            }
            yield return new WaitForSeconds(Settings.BonusTimeRespawn);
            StartCoroutine(PutBonusesOnMap());
        }

        private IEnumerator PutWeaponsOnMap()
        {
            for (var i = 0; i < Settings.WeaponInitialLying; i++)
            {
                PutItemOnMap(PoolManager.instance.Stones);
            }
            for (var i = 0; i < Settings.CountLyingSkulls; i++)
            {
                PutItemOnMap(PoolManager.instance.Skulls);
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
            if (!isServerPlayer && !isAiPlayer)
            {
                BattleGui.instance.SubscribeOnEvents(playerModel);
                smoothCamera.target = prefab.transform;
                smoothCamera.SetPlayer(prefab.GetComponent<PlayerModelBase>());
                if (serverNotify != null) playerModel.GetComponent<SpriteRenderer>().material.color = Color.red;

            }
            playerModel.Init(player, r,  serverNotify);
            PlayerPool.instance.Add(player.Id, playerModel);
      
            playerModel.ChangedWeaponsPool += PoolManager.instance.SwitchPoolWeapons;
            playerModel.Birth(new Vector2(r.Next(1, Settings.WidthMap - 1), r.Next(1, Settings.HeightMap - 1)));
        }      
    }
}
