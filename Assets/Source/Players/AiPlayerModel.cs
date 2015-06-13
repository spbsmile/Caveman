using Caveman.Setting;
using Caveman.Utils;
using UnityEngine;
using Random = System.Random;

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

        public void InitAi(Player player, Vector2 start, Random random, PlayerPool pool, Transform[] allLyingWeapons)
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

        private Vector2 FindClosestLyingWeapon(params Transform[] array)
        {
            float minDistance = 0;
            var nearPosition = Vector2.zero;
            for (var i = 0; i < array.Length; i++)
            {
                if (!array[i].gameObject.activeSelf) continue;
                if (minDistance < 0.1f)
                {
                    minDistance = Vector2.Distance(array[i].position, transform.position);
                    nearPosition = array[i].position;
                }
                else
                {
                    var childDistance = Vector2.Distance(array[i].position, transform.position);
                    if (minDistance > childDistance)
                    {
                        minDistance = childDistance;
                        nearPosition = array[i].position;
                    }
                }
            }
            return nearPosition;
        }

        private Vector2 RandomPosition
        {
            get { return new Vector2(r.Next(-Settings.Br, Settings.Br), r.Next(-Settings.Br, Settings.Br)); }
        }
    }
}


