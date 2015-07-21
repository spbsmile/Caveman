using System.Collections;
using Caveman.Setting;
using Caveman.Utils;
using UnityEngine;
using Random = System.Random;

namespace Caveman.Players
{
    public class AiPlayerModel : PlayerModelClient
    {
        private Transform weapons;

        public void OnDisable()
        {
            StandStill();
        }

        public override IEnumerator Respawn()
        {
            SetRandomMove();
            return base.Respawn();
        }

        protected override void Start()
        {
            base.Start();
            GetComponent<SpriteRenderer>().color = new Color32((byte) r.Next(255), (byte) r.Next(255),
                (byte) r.Next(255), 255);
            SetRandomMove();
        }

        public void InitAi(Player player, Vector2 start, Random random, PlayerPool pool, Transform allLyingWeapons)
        {
            Init(player, start, random, pool, null);
            weapons = allLyingWeapons;
        }

        public void Update()
        {
            if ((Vector2.SqrMagnitude(delta) > UnityExtensions.ThresholdPosition &&
                    Vector2.SqrMagnitude((Vector2)transform.position - target) > UnityExtensions.ThresholdPosition))
            {
                Move();
            }
            else
            {
                if (player.Weapons < Settings.MaxCountWeapons)
                {
                    target = FindClosestLyingWeapon;
                    if (target == Vector2.zero)
                    {
                        target = RandomPosition;
                    }
                    delta = UnityExtensions.CalculateDelta(transform.position, target, Speed);
                    animator.SetFloat(delta.y > 0 ? Settings.AnimRunB : Settings.AnimRunF, Speed);
                }
                else
                {
                    target = RandomPosition;
                    delta = UnityExtensions.CalculateDelta(transform.position, target, Speed);
                    animator.SetFloat(delta.y > 0 ? Settings.AnimRunB : Settings.AnimRunF, Speed);
                }
            }
        }

        private Vector2 FindClosestLyingWeapon
        {
            get
            {
                var minDistance = (float)Settings.HeightMap*Settings.WidthMap;
                var nearPosition = Vector2.zero;

                foreach (Transform weapon in weapons)
                {
                    if (!weapon.gameObject.activeSelf) continue;
                    var childDistance = Vector2.SqrMagnitude(weapon.position - transform.position);
                    if (minDistance > childDistance)
                    {
                        minDistance = childDistance;
                        nearPosition = weapon.position;
                    }
                }
                return nearPosition;
            }
        }

        private void SetRandomMove()
        {
            target = RandomPosition;
            delta = UnityExtensions.CalculateDelta(transform.position, target, Settings.SpeedPlayer);
            animator.SetFloat(delta.y > 0 ? Settings.AnimRunB : Settings.AnimRunF, Settings.SpeedPlayer);
        }

        private Vector2 RandomPosition
        {
            get { return new Vector2(r.Next(Settings.WidthMap), r.Next(Settings.HeightMap)); }
        }
    }
}


