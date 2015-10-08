using System.Collections;
using Caveman.Players;
using Caveman.Specification;
using Caveman.Utils;
using Caveman.Weapons;
using UnityEngine;

namespace Caveman.Bonuses
{
    public class BonusBase : ASupportPool<BonusBase>
    {
        protected float preValue;
        protected ObjectPool<BonusBase> pool;

        public BonusSpecification Specification { protected set; get; } 

        public void OnTriggerEnter2D(Collider2D other)
        {
            if (Time.time < 1) return; // todo подумать. 
            if (other.gameObject.GetComponent<PlayerModelBase>())
            {
                Effect(other.gameObject.GetComponent<PlayerModelBase>());
            }
            if (other.gameObject.GetComponent<WeaponModelBase>())
            {
                pool.Store(this);
            }
        }

        public override void SetPool(ObjectPool<BonusBase> item)
        {
            pool = item;
        }

        public virtual void Effect(PlayerModelBase playerModel)
        {
            playerModel.Player.PickUpBonus(Specification.Type, Specification.Duration);
            //todo внедрить систему событий
            playerModel.bonusBase = this;
            transform.position = new Vector3(200, 200, 200);
            StartCoroutine(UnEffect(playerModel));
        }

        protected virtual IEnumerator UnEffect(PlayerModelBase playerModel)
        {
            yield return null;
        }
    }
}