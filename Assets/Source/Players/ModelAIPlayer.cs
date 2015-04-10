using UnityEngine;

namespace Caveman.Players
{
    public class ModelAIPlayer : ModelBasePlayer
    {
        private Transform ContainerWeapons;

        public void Start()
        {
            transform.position = new Vector3(3, 3);
        }

        public void Update()
        {
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


