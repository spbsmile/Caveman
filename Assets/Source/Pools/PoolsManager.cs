using System.Collections.Generic;
using Caveman.Bonuses;
using Caveman.CustomAnimation;
using Caveman.Setting;
using Caveman.Configs;
using Caveman.Utils;
using Caveman.Weapons;
using UnityEngine;

namespace Caveman.Pools
{
    public class PoolsManager : MonoBehaviour
    {
        public static PoolsManager instance;

        public StoneSplash prefabStoneFlagmentInc;
        public EffectBase prefabDeathImage;

        public Transform parentAllContainers;

        public ObjectPool<EffectBase> SplashesStone { private set; get; }
        public ObjectPool<EffectBase> ImagesDeath { private set; get; }
        public ObjectPool<EffectBase> Axes { private set; get; }
        public ObjectPool<WeaponModelBase> Stones { private set; get; }
        public ObjectPool<WeaponModelBase> Skulls { private set; get; }
        public ObjectPool<BonusBase> BonusesSpeed { private set; get; }

        /// <summary>
        /// only for procedure generate 
        /// </summary>
        public Dictionary<string, object> Pools = new Dictionary<string, object>();

        public void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
        }

        //todo also configs pools gameobjects from json
        public void PrepareAllPools(CurrentGameSettings currentSettings)
        {

            ImagesDeath = PreparePool<EffectBase>(, containerDeathImages,
                Instantiate(Resources.Load("enemy", typeof (GameObject))) as EffectBase);
            //SplashesStone = PreparePool<EffectBase>()
        }

        private GameObject CreateContainer(Transform parent, string name)
        {
            var c = new GameObject(name);
            c.transform.parent = parent;
            return c;
        }
    

    /// Used object pool pattern
        private ObjectPool<T> PreparePool<T>(int initialBufferSize, Transform container, T prefab) where T : MonoBehaviour
        {
            var pool = container.GetComponent<ObjectPool<T>>();
            pool.CreatePool(prefab, initialBufferSize);
            for (var i = 0; i < initialBufferSize; i++)
            {
                var item = Instantiate(prefab);

                item.transform.SetParent(container);
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
