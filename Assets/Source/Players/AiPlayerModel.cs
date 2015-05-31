using System;
using Caveman.Setting;
using Caveman.Utils;
using UnityEngine;

namespace Caveman.Players
{
    public class AiPlayerModel : BasePlayerModel
    {
        private Transform[] weapons;

        protected override void Start()
        {
            base.Start();
            GetComponent<SpriteRenderer>().color = new Color32((byte) r.Next(255), (byte) r.Next(255),
                (byte) r.Next(255), 255);
            target = RandomPosition;
            delta = UnityExtensions.CalculateDelta(transform.position, target, Settings.SpeedPlayer);
            animator.SetFloat(delta.y > 0 ? Settings.AnimRunB : Settings.AnimRunF, Settings.SpeedPlayer);
        }

        public void Update()
        {
            ThrowStoneOnTimer();

            if (!InMotion)
            {
                if (player.Weapons < Settings.MaxCountWeapons)
                {
                    target = FindClosestLyingWeapon(weapons);
                    if (target == Vector2.zero)
                    {
                        target = RandomPosition;
                    }
                    delta = UnityExtensions.CalculateDelta(transform.position, target, Settings.SpeedPlayer);
                    animator.SetFloat(delta.y > 0 ? Settings.AnimRunB : Settings.AnimRunF, Settings.SpeedPlayer);
                }
                else
                {
                    target = RandomPosition;
                    delta = UnityExtensions.CalculateDelta(transform.position, target, Settings.SpeedPlayer);
                    animator.SetFloat(delta.y > 0 ? Settings.AnimRunB : Settings.AnimRunF, Settings.SpeedPlayer);
                }
                InMotion = !InMotion;
            }
            else
            {
                Move();
            }
        }

        public void SetWeapons(Transform[] weapons)
        {
            this.weapons = weapons;
        }

        private Vector2 RandomPosition
        {
            get { return new Vector2(r.Next(-Settings.Br, Settings.Br), r.Next(-Settings.Br, Settings.Br)); }
        }
    }
}


