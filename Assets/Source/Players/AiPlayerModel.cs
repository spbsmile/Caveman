using Caveman.Setting;
using Caveman.Utils;
using UnityEngine;

namespace Caveman.Players
{
    public class AiPlayerModel : PlayerModelClient
    {
        private Transform weapons;

        protected override void Start()
        {
            base.Start();
            GetComponent<SpriteRenderer>().color = new Color32((byte) r.Next(255), (byte) r.Next(255),
                (byte) r.Next(255), 255);
            SetMove(RandomPosition);
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
                if (player.Weapons < weaponSpecification.MaxOnPLayer)
                {
                    target = FindClosestLyingWeapon;
                    if (target == Vector2.zero)
                    {
                        target = RandomPosition;
                    }
                    SetMove(target);
                }
                else
                {
                    SetMove(RandomPosition);
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

        private Vector2 RandomPosition
        {
            get { return new Vector2(r.Next(Settings.WidthMap), r.Next(Settings.HeightMap)); }
        }

        public void SetWeapons(Transform containerStones)
        {
            weapons = containerStones;
        }
    }
}


