using UnityEngine;

namespace Caveman.Players
{
    public class ModelAIPlayer : ModelBasePlayer
    {
        private const int MaxCountWeapons = 4;
        //todo boundary tileMap
        private const int TempBoundary = 7;

        private Transform ContainerWeapons;

        public override void Start()
        {
            base.Start();
            GetComponent<SpriteRenderer>().color = new Color32((byte) random.Next(255), (byte) random.Next(255),
                (byte) random.Next(255), 255);
            target = new Vector2(random.Next(-TempBoundary, TempBoundary), random.Next(-TempBoundary, TempBoundary));
            delta = UnityExtensions.CalculateDelta(transform.position, target, Settings.SpeedPlayer);
            animator.SetFloat("Speed", Settings.SpeedPlayer);
        }

        public void Update()
        {
            ThrowStoneOnTimer();

            if (MoveStop())
            {
                if (player.weapons < MaxCountWeapons)
                {
                    target = FindClosest(ContainerWeapons);
                    //todo if target null random move
                    delta = UnityExtensions.CalculateDelta(transform.position, target, Settings.SpeedPlayer);
                }
                else
                {
                    target = new Vector2(random.Next(-TempBoundary, TempBoundary), random.Next(-TempBoundary, TempBoundary));
                    delta = UnityExtensions.CalculateDelta(transform.position, target, Settings.SpeedPlayer);
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
    }
}


