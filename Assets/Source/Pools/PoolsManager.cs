using System;
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

        public ObjectPool<ImageBase> SplashesStone { private set; get; }
        public ObjectPool<ImageBase> ImagesDeath { private set; get; }
        public ObjectPool<WeaponModelBase> Axes { private set; get; }
        public ObjectPool<WeaponModelBase> Stones { private set; get; }
        // todo miss prefab skull in resource
        public ObjectPool<WeaponModelBase> Skulls { private set; get; }
        public ObjectPool<BonusBase> BonusesSpeed { private set; get; }

        public readonly Dictionary<string, object> Pools = new Dictionary<string, object>();

        public void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
        }

        public void PrepareAllPools(CurrentGameSettings settings)
        {
            var poolsConfig = settings.PoolsConfigs["sample"];

            var deathConfig = settings.ImagesConfigs["death"];
            var splahesConfig = settings.ImagesConfigs["stones_fragment"];
            var stonesConfig = settings.WeaponsConfigs["stone"];
            var axesConfig = settings.WeaponsConfigs["axe"];
            var skullsConfig = settings.WeaponsConfigs["skulls"];
            var bonusSpeedConfig = settings.BonusesConfigs["speed"];

            ImagesDeath = PreparePool(containerImagesDeath, Inst<ImageBase>(deathConfig.PrefabPath), poolsConfig.ImagesOrdinary, null);
            SplashesStone = PreparePool(containerSplashStones, Inst<ImageBase>(splahesConfig.PrefabPath), poolsConfig.ImagesPopular, null);
            Skulls = PreparePool(containerSkulls, Inst<WeaponModelBase>(skullsConfig.PrefabPath), poolsConfig.WeaponsOrdinary, InitializationPoolAxe);
            Stones = PreparePool(containerStones, Inst<WeaponModelBase>(stonesConfig.PrefabPath), poolsConfig.WeaponsPopular, InitializationPoolStone);
            //Axes = PreparePool(Inst<WeaponModelBase>(axesConfig.PrefabPath), poolsConfig.BonusesOrdinary);
            BonusesSpeed = PreparePool(containerBonusesSpeed, Inst<BonusBase>(bonusSpeedConfig.PrefabPath), poolsConfig.BonusesOrdinary, InitializationPoolBonus);

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

        private ObjectPool<T> PreparePool<T>(Transform container, T prefab, int initialBufferSize, Action<GameObject, ObjectPool<T>> init)
            where T : MonoBehaviour
        {
            var pool = container.GetComponent<ObjectPool<T>>();
            pool.Initialization(prefab, initialBufferSize);
            for (var i = 0; i < initialBufferSize; i++)
            {
                var item = Instantiate(prefab);
                if (init != null)
                {
                    init(item.gameObject, pool);
                }
                item.transform.SetParent(container.transform);
                pool.Store(item);
            }
            return pool;
        }

        /// <summary>
        /// Each model assigned reference on objectpool
        /// </summary>
        private void InitializationPoolBonus(GameObject item, ObjectPool<BonusBase> pool)
        {
            item.GetComponent<BonusBase>().InitializationPool(pool);
        }

        private void InitializationPoolAxe(GameObject item, ObjectPool<WeaponModelBase> pool)
        {
            item.GetComponent<AxeModel>().InitializationPool(pool);
        }

        private void InitializationPoolStone(GameObject item, ObjectPool<WeaponModelBase> pool)
        {
            var model = item.GetComponent<StoneModel>();
            model.InitializationPool(pool);
            model.InitializationPoolSplashesStone(SplashesStone);
        }

        /// <summary>
        /// When player pickup weapon another type 
        /// </summary>
        public ObjectPool<WeaponModelBase> ChangeWeaponPool(WeaponConfig.Types type)
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
