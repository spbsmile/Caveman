using Caveman.Players;
using Caveman.Setting;
using Caveman.Utils;
using UnityEngine;
using Random = System.Random;

namespace Caveman.Bonuses
{
    public class BonusBase : MonoBehaviour
    {
        protected Animator animator;

        private float duration;
        private ObjectPool pool;
        private Random r;

        public void Start()
        {
            animator = GetComponent<Animator>();
        }

        public void Init(Random random, ObjectPool pool, string name)
        {
            this.name = name;
            this.pool = pool;
            r = random;
        }

        public void RandomPosition()
        {
            transform.position = new Vector2(r.Next(-Settings.Br, Settings.Br), r.Next(-Settings.Br, Settings.Br));
        }

        public void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.GetComponent<PlayerModelBase>())
            {
            }
        }

        public virtual void Effect(PlayerModelBase playerModel)
        {
            //playerModel.Speed = playerModel.Speed*2;
            ///// no func
            //playerModel.ResetBonus -= playerModel.Speed = playerModel.Speed/2;
        }
    }
}