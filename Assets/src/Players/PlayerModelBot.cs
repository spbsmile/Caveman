using Caveman.Utils;
using UnityEngine;
using Random = System.Random;

namespace Caveman.Players
{
    public class PlayerModelBot : PlayerModelClient
    {
        private Vector2 targetPosition;
        private float maxDistance;
        private Random rand;
        private Transform containerStones;

        protected void Start()
        {
            GetComponent<SpriteRenderer>().color = new Color32((byte) rand.Next(255), (byte) rand.Next(255),
                (byte) rand.Next(255), 255);
        }

        public void Update()
        {
            if (Vector2.SqrMagnitude(moveUnit) > UnityExtensions.ThresholdPosition &&
                Vector2.SqrMagnitude((Vector2)transform.position - targetPosition) > UnityExtensions.ThresholdPosition)
            {
                Move();
            }
            else
            {
                if (IsEnoughStrength(WeaponConfig.Weight))
                {
                    var closestPosition = FindClosestLyingWeapon;
                    targetPosition = closestPosition == Vector2.zero ? GetRandomPosition() : closestPosition;
                    CalculateMoveUnit(targetPosition);
                }
                else
                {
                    targetPosition = GetRandomPosition();
                    CalculateMoveUnit(targetPosition);
                }
            }
        }

        public void Initialization(Random rand, float maxDistance, Transform containerStones)
        {
            this.rand = rand;
            this.maxDistance = maxDistance;
            this.containerStones = containerStones;
        }
     
        private Vector2 FindClosestLyingWeapon
        {
            get
            {
                var minDistance = maxDistance;
                var nearPosition = Vector2.zero;

                foreach (Transform weapon in containerStones)
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
    }
}


