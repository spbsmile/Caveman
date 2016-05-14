using Caveman.Bonuses;
using Caveman.CustomAnimation;
using Caveman.Setting;
using Caveman.Specification;
using Caveman.Weapons;
using UnityEngine;

namespace Caveman.Pools
{
    public class PoolManager : MonoBehaviour
    {
        public static PoolManager instance;

        public AxeModel prefabAxe;
        public StoneModel prefabStone;
        public StoneSplash prefabStoneFlagmentInc;
        public EffectBase prefabDeathImage;
        public SpeedBonus prefabBonusSpeed;

        public Transform containerStones;
        public Transform containerSplashStones;
        public Transform containerSkulls;
        public Transform containerDeathImages;
        //public Transform containerPlayers;
        public Transform containerBonusesSpeed;

        public ObjectPool<EffectBase> SplashesStone { private set; get; }

        public ObjectPool<EffectBase> ImagesDeath { private set; get; }
        public ObjectPool<WeaponModelBase> Stones { private set; get; }
        public ObjectPool<WeaponModelBase> Skulls { private set; get; }
        public ObjectPool<BonusBase> BonusesSpeed { private set; get; }

        private enum PoolName
        {
            Stones,
            ImagesDeath,
            Skulls,
            BonusesSpeed,
            Axe,
            StoneFlagment
        }

        public void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }

            PreparePool(Settings.PoolCountDeathImages, containerDeathImages, prefabDeathImage, PoolName.ImagesDeath);
            //PreparePool();

        }

        /// Used object pool pattern
        private void PreparePool<T>(int initialBufferSize, Transform container, T prefab,
             PoolName name) where T : MonoBehaviour
        {
            var pool = container.GetComponent<ObjectPool<T>>();
            pool.CreatePool(prefab, initialBufferSize);
            for (var i = 0; i < initialBufferSize; i++)
            {
                var item = Instantiate(prefab);
              
                item.transform.SetParent(container);
                pool.Store(item);
            }
            //switch (PoolName)
            //{
                    
            //}
        }

        /// <summary>
        /// When player pickup weapon another type 
        /// </summary>
        public ObjectPool<WeaponModelBase> SwitchPoolWeapons(WeaponSpecification.Types type)
        {
            switch (type)
            {
                case WeaponSpecification.Types.Stone:
                    return Stones;
                case WeaponSpecification.Types.Skull:
                    return Skulls;
            }
            return null;
        }
    }
}
