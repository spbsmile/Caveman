using System;
using System.Diagnostics.PerformanceData;
using Caveman.Bonuses;
using Caveman.CustomAnimation;
using Caveman.Specification;
using Caveman.Weapons;
using UnityEngine;

namespace Caveman.Pools
{
    public class PoolManager : MonoBehaviour
    {
        public static PoolManager instance;

        public Transform containerStones;
        public Transform containerSplashStones;
        public Transform containerSkulls;
        public Transform containerDeathImages;
        public Transform containerPlayers;
        public Transform containerBonusesSpeed;

        public ObjectPool<EffectBase> SplashesStone {  get { return poolStonesSplash; } }
        public ObjectPool<EffectBase> ImagesDeath { private set; get; }
        public ObjectPool<WeaponModelBase> Stones { private set; get; }
        public ObjectPool<WeaponModelBase> Skulls { private set; get; }
        public ObjectPool<BonusBase> BonusesSpeed { private set; get; }

        private PlayerPool poolPlayers;
        private ObjectPool<WeaponModelBase> poolStones;
        private ObjectPool<WeaponModelBase> poolSkulls;
        private ObjectPool<BonusBase> poolBonusesSpeed;
        private ObjectPool<EffectBase> poolStonesSplash;
        private ObjectPool<EffectBase> poolDeathImage;




        /// <summary>
        /// Used object pool pattern
        /// </summary>
        public void PreparePool<T>(int initialBufferSize, Transform container, T prefab,
            Action<GameObject, ObjectPool<T>> init) where T : MonoBehaviour
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


        }

        /// <summary>
        /// When player pickup weapon another type 
        /// </summary>
        private ObjectPool<WeaponModelBase> SwitchPoolWeapons(WeaponSpecification.Types type)
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

    }
}
