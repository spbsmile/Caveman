using System;
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

        private float duration;
        private float value;
        private ObjectPool pool;
        private Random r;
        private Transform icon;

        public void Start()
        {
            animator = GetComponent<Animator>();
        }

        public void Init(Random random, ObjectPool pool, Transform icon)
        {
            this.icon = icon;
            this.pool = pool;
            r = random;
        }

        public void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.GetComponent<PlayerModelBase>())
            {
                Effect(other.gameObject.GetComponent<PlayerModelBase>());
            }
            if (other.gameObject.GetComponent<WeaponModelBase>())
            {
                pool.Store(transform);
            }
        }

        public virtual void Effect(PlayerModelBase playerModel)
        {
            ChangedBonus(icon);
            pool.Store(transform);
        }
    }
}