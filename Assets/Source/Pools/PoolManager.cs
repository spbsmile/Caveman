using System;
using System.Collections;
using Caveman.Setting;
using Caveman.Utils;
using UnityEngine;

namespace Caveman.Pools
{
    public class PoolManager : MonoBehaviour
    {    
         public ObjectPool<EffectBase> SplashesStone { private set; get; }
         public ObjectPool<EffectBase> ImagesDeath { private set; get; }
         public ObjectPool<WeaponModelBase> Stones { private set; get; }
         public ObjectPool<WeaponModelBase> Skulls { private set; get; }
         public ObjectPool<BonusBase> BonusesSpeed { private set; get; }
        
       
        
               
        /// <summary>
        /// Used object pool pattern
        /// </summary>
        public ObjectPool<T> PreparePool<T>(int initialBufferSize, Transform container, T prefab, Action<GameObject, ObjectPool<T>> init) where T : MonoBehaviour
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
