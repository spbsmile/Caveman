using System;
using System.Collections;
using Caveman.Players;
using Caveman.Utils;
using Caveman.Weapons;
using UnityEngine;
using Random = System.Random;

namespace Caveman.Bonuses
{
    public class BonusBase : ISupportPool
    {
        public Action<Transform> ChangedBonus;

        protected Animator animator;
        protected float duration;
        protected float value;
        protected float preValue;
        //todo hack private
        protected ObjectPool pool;
        private Random r;
        private Transform icon;

        public void Start()
        {
            animator = GetComponent<Animator>();
        }

        public void Init(ObjectPool poolBonuses, Random random, Transform icon)
        {
            this.icon = icon;
            pool = poolBonuses;
            r = random;
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
                pool.Store(transform);
            }
        }

        public override void SetPool(ObjectPool item)
        {
            pool = item;
        }

        protected virtual void Effect(PlayerModelBase playerModel)
        {
            //ChangedBonus(icon);
            //todo hack, внедрить систему событий
            if (playerModel.bonusType != null ) return;
            playerModel.bonusType = this;
            transform.position = new Vector3(200, 200, 200);
            StartCoroutine(UnEffect(playerModel));
        }

        protected virtual IEnumerator UnEffect(PlayerModelBase playerModel)
        {
            //ChangedBonus(icon);
            yield return null;
        }
    }
}