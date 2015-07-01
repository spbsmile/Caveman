using System;
using System.Collections;
using Caveman.Players;
using Caveman.Setting;
using Caveman.Utils;
using Caveman.Weapons;
using UnityEngine;
using Random = System.Random;

namespace Caveman.Bonuses
{
    public class BonusBase : MonoBehaviour
    {
        public Action<Transform> ChangedBonus;

        protected Animator animator;
        protected float duration;
        protected float value;
        protected float preValue;

        private ObjectPool pool;
        private Random r;
        private Transform icon;

        public void Start()
        {
            animator = GetComponent<Animator>();
        }

        public void Init(ObjectPool pool, Random random, Transform icon)
        {
            this.icon = icon;
            this.pool = pool;
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

        protected virtual void Effect(PlayerModelBase playerModel)
        {
            //ChangedBonus(icon);
            pool.Store(transform);
            gameObject.SetActive(true);
            StartCoroutine(UnEffect(playerModel));
        }

        protected virtual IEnumerator UnEffect(PlayerModelBase playerModel)
        {
            //ChangedBonus(icon);
            yield return new WaitForSeconds(Settings.DurationBonusSpeed);
        }
    }
}