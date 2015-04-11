using UnityEngine;

namespace Caveman.Players
{
    public class ModelBasePlayer : MonoBehaviour
    {
        public Player player;

        public void Init(Player player, Vector2 position)
        {
            name = player.name;
            this.player = player;
            transform.position = position;
        }

        public void OnTriggerEnter2D(Collider2D other)
        {
            print("hello");
            if (Time.time < 1) return;
            var weapon = other.gameObject.GetComponent<WeaponModel>();
            if (weapon != null)
            {
                if (weapon.owner == null)
                {
                    player.weapons++;
                    Destroy(other.gameObject);
                }
                else
                {
                    if (weapon.owner != player)
                    {
                        print("killed");
                        weapon.owner.killed++;
                        Destroy(other.gameObject);
                        Destroy(gameObject);
                    }
                }
            }
        }
    }
}


