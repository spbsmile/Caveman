using Caveman.Setting;
using Caveman.Utils;
using UnityEngine;

namespace Caveman.Players
{
    public class AiPlayerModel : BasePlayerModel
    {
        private const int MaxCountWeapons = 4;
        private Transform ContainerWeapons;

        public override void Start()
        {
            base.Start();
            GetComponent<SpriteRenderer>().color = new Color32((byte) random.Next(255), (byte) random.Next(255),
                (byte) random.Next(255), 255);
            target = RandomPosition;
            delta = UnityExtensions.CalculateDelta(transform.position, target, Settings.SpeedPlayer);
            animator.SetFloat(delta.y > 0 ? Settings.AnimRunB : Settings.AnimRunF, Settings.SpeedPlayer);
        }

        public void Update()
        {
            ThrowStoneOnTimer();

            if (MoveStop())
            {
                if (player.weapons < MaxCountWeapons)
                {
                    target = FindClosest(ContainerWeapons);
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
            }
            else
            {
                Move();
            }
        }

        public void SetWeapons(Transform weapons)
        {
            ContainerWeapons = weapons;
        }

        private Vector2 RandomPosition
        {
            get { return new Vector2(random.Next(-Settings.BoundaryRandom, Settings.BoundaryRandom), random.Next(-Settings.BoundaryRandom, Settings.BoundaryRandom)); }
        }
    }
}


