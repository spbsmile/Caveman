using UnityEngine;

namespace Caveman.Players
{
    public class ModelBasePlayer : MonoBehaviour
    {
        public Player player;

        public void Ini(Player player, Vector2 position)
        {
            name = player.name;
            this.player = player;
            transform.position = position;
        }

        public void OnTriggerEnter2D(Collider2D other)
        {
            if (Time.time < 1) return;
            if (other.gameObject.GetComponent<WeaponModel>())
            {
                player.weapons++;
                Destroy(other.gameObject);
            }
        }
    }
}


