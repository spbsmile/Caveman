using UnityEngine;

namespace Caveman.Players
{
    public class ModelAIPlayer : ModelBasePlayer
    {
        private Transform ContainerWeapons;

        private int timeThrowStone = 300;

        public void Update()
        {
            if (timeThrowStone-- < 0)
            {
                ThrowStone();
                timeThrowStone = 300;
            }
            //delay
            foreach (Transform weapon in ContainerWeapons)
            {
                //weapon.position
            }
        }

        public void SetWeapons(Transform weapons)
        {
            ContainerWeapons = weapons;
        }
    }
}


