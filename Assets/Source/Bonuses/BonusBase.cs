using System.Collections;
using Caveman.Players;
using Caveman.Utils;
using Caveman.Weapons;
using UnityEngine;

namespace Caveman.Bonuses
{
    public enum BonusType
    {
        Speed,
        Shield
    }

    public class BonusBase : ASupportPool<BonusBase>
    {
        public int duration;
        public virtual BonusType Type { get { return BonusType.Speed; }}

        protected Animator animator;
        protected float value;
        protected float preValue;

        protected ObjectPool<BonusBase> pool;
        private Transform icon;

        public void Start()
        {
            animator = GetComponent<Animator>();
        }

        public void Init(ObjectPool<BonusBase> poolBonuses, int duration)
        {
            this.duration = duration;
            pool = poolBonuses;
        }

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
            playerModel.player.PickUpBonus(Type, duration);
            //todo внедрить систему событий
            playerModel.bonusType = this;
            transform.position = new Vector3(200, 200, 200);
            StartCoroutine(UnEffect(playerModel));
        }

        protected virtual IEnumerator UnEffect(PlayerModelBase playerModel)
        {
            yield return null;
        }
    }
}