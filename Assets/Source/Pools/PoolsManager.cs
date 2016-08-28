using System.Collections.Generic;
using Caveman.Bonuses;
using Caveman.CustomAnimation;
using Caveman.Setting;
using Caveman.Configs;
using Caveman.Weapons;
using UnityEngine;

namespace Caveman.Pools
{
    public class PoolsManager : MonoBehaviour
    {
        public static PoolsManager instance;                

        public Transform containerStones;
        public Transform containerSplashStones;
        public Transform containerSkulls;
        public Transform containerImagesDeath;
        public Transform containerBonusesSpeed;

        public ObjectPool<EffectBase> SplashesStone { private set; get; }
        public ObjectPool<EffectBase> ImagesDeath { private set; get; }
        public ObjectPool<WeaponModelBase> Axes { private set; get; }
        public ObjectPool<WeaponModelBase> Stones { private set; get; }
        // todo miss prefab skull in resource
        public ObjectPool<WeaponModelBase> Skulls { private set; get; }
        public ObjectPool<BonusBase> BonusesSpeed { private set; get; }

        /// <summary>
        /// only for procedure generate 
        /// </summary>
        public readonly Dictionary<string, object> Pools = new Dictionary<string, object>();

        public void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
        }

        //todo also configs pools gameobjects from json
        public void PrepareAllPools(CurrentGameSettings settings)
        {
            var poolsConfig = settings.PoolsConfigs["sample"];

            var deathConfig = settings.ImagesConfigs["death"];
            var splahesConfig = settings.ImagesConfigs["stones_fragment"];
            var stonesConfig = settings.WeaponsConfigs["stone"];
            var axesConfig = settings.WeaponsConfigs["axe"];
            var skullsConfig = settings.WeaponsConfigs["skulls"];
            var bonusSpeedConfig = settings.BonusesConfigs["speed"];

            ImagesDeath = PreparePool(containerImagesDeath, Inst<EffectBase>(deathConfig.PrefabPath),  poolsConfig.ImagesOrdinary);
            SplashesStone = PreparePool(containerSplashStones, Inst<EffectBase>(splahesConfig.PrefabPath),  poolsConfig.ImagesPopular);
            Skulls = PreparePool(containerSkulls, Inst<WeaponModelBase>(skullsConfig.PrefabPath), poolsConfig.WeaponsOrdinary);
            Stones = PreparePool(containerStones, Inst<WeaponModelBase>(stonesConfig.PrefabPath), poolsConfig.WeaponsPopular);
            //Axes = PreparePool(Inst<WeaponModelBase>(axesConfig.PrefabPath), poolsConfig.BonusesOrdinary);
            BonusesSpeed = PreparePool(containerBonusesSpeed, Inst<BonusBase>(bonusSpeedConfig.PrefabPath), poolsConfig.BonusesOrdinary);

            Pools.Add(deathConfig.PrefabPath, ImagesDeath);
            Pools.Add(splahesConfig.PrefabPath, SplashesStone);
            Pools.Add(stonesConfig.PrefabPath, Stones);
            Pools.Add(axesConfig.PrefabPath, Axes);
            Pools.Add(bonusSpeedConfig.PrefabPath, BonusesSpeed);
        }

        private T Inst<T>(string prefabPath) where T : MonoBehaviour
        {           
            return Instantiate(Resources.Load(prefabPath, typeof (T))) as T;
        }

        /// <summary>
        /// Used object pool pattern
        /// </summary>
        private ObjectPool<T> PreparePool<T>(Transform container, T prefab, int initialBufferSize)
            where T : MonoBehaviour
        {

            var pool = container.GetComponent<ObjectPool<T>>();
            pool.CreatePool(prefab, initialBufferSize);
                        
            pool.CreatePool(prefab, initialBufferSize);
            for (var i = 0; i < initialBufferSize; i++)
            {
                var item = Instantiate(prefab);
                item.transform.SetParent(container.transform);
                pool.Store(item);
            }
            return pool;
        }       

        /// <summary>
        /// When player pickup weapon another type 
        /// </summary>
        public ObjectPool<WeaponModelBase> SwitchPoolWeapons(WeaponConfig.Types type)
        {
            switch (type)
            {
                case WeaponConfig.Types.Stone:
                    return Stones;
                case WeaponConfig.Types.Skull:
                    return Skulls;
            }
            return null;
        }
    }
}
