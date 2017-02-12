using System.Collections;
using Caveman.Configs;
using Caveman.Players;
using Caveman.Pools;
using Caveman.Weapons;
using UnityEngine;

namespace Caveman.Bonuses
{
    public class BonusBase : ASupportPool<BonusBase>
    {
        protected ObjectPool<BonusBase> pool;

        public BonusConfig Config { protected set; get; } 

        public void OnTriggerEnter2D(Collider2D other)
        {
            if (Time.time < 1) return; // todo 
            if (other.gameObject.GetComponent<PlayerModelBase>())
            {
                Effect(other.gameObject.GetComponent<PlayerModelBase>().PlayerCore);
            }
            else if (other.gameObject.GetComponent<WeaponModelBase>())
            {
                pool.Store(this);
            }
        }

        public override void InitializationPool(ObjectPool<BonusBase> item)
        {
            pool = item;
        }

        public virtual void Effect(ISupportBonus item)
        {
            // HACK: trigger methods calling before Start
            if (item == null || Config == null)
                return;
            item.ActivatedBonus(Config.Type, Config.Duration);
            transform.position = new Vector3(200, 200, 200);
            StartCoroutine(UnEffect(item));
        }

        protected virtual IEnumerator UnEffect(ISupportBonus model)
        {
            yield return null;
        }
    }
}