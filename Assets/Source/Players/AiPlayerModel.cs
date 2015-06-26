using Caveman.Setting;
using Caveman.Utils;
using UnityEngine;
using Random = System.Random;

namespace Caveman.Players
{
    public class AiPlayerModel : PlayerModelBase
    {
        private Transform weapons;

        protected override void Start()
        {
            base.Start();
            GetComponent<SpriteRenderer>().color = new Color32((byte) r.Next(255), (byte) r.Next(255),
                (byte) r.Next(255), 255);
            target = RandomPosition;
            delta = UnityExtensions.CalculateDelta(transform.position, target, Settings.SpeedPlayer);
            animator.SetFloat(delta.y > 0 ? Settings.AnimRunB : Settings.AnimRunF, Settings.SpeedPlayer);
        }

        public void InitAi(Player player, Vector2 start, Random random, PlayerPool pool, Transform allLyingWeapons)
        {
            Init(player, start, random, pool);
            weapons = allLyingWeapons;
        }

        public void Update()
        {
            ThrowStoneOnTimer();

            if (!InMotion)
            {
                if (player.Weapons < Settings.MaxCountWeapons)
                {
                    target = FindClosestLyingWeapon;
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

        private Vector2 FindClosestLyingWeapon
        {
            get
            {
                float minDistance = 0;
                var nearPosition = Vector2.zero;

                foreach (Transform weapon in weapons)
                {
                    if (!weapon.gameObject.activeSelf) continue;
                    if (minDistance < 0.1f)
                    {
                        minDistance = Vector2.Distance(weapon.position, transform.position);
                        nearPosition = weapon.position;
                    }
                    else
                    {
                        var childDistance = Vector2.Distance(weapon.position, transform.position);
                        if (minDistance > childDistance)
                        {
                            minDistance = childDistance;
                            nearPosition = weapon.position;
                        }
                    }
                }
                return nearPosition;
            }
        }

        private Vector2 RandomPosition
        {
            get { return new Vector2(r.Next(-Settings.Br, Settings.Br), r.Next(-Settings.Br, Settings.Br)); }
        }
    }
}


