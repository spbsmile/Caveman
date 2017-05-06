using Caveman.Players;
using UnityEngine;

namespace Caveman.Level
{
    public class Chest : MonoBehaviour
    {
        [SerializeField] private Sprite chestOpenIcon;

        private SpriteRenderer render;

        private State currentState;

        private enum State
        {
            Close,
            Open
        }

        public void Start()
        {
            render = GetComponent<SpriteRenderer>();
            currentState = State.Close;
        }

        public void OnTriggerEnter2D(Collider2D other)
        {
            if (currentState == State.Close && other.GetComponent<PlayerModelHero>())
            {
                var owner = other.GetComponent<PlayerModelHero>().Core;
                owner.ActivatedChest(() =>
                {
                    currentState = State.Open;
                    render.sprite = chestOpenIcon;
                    owner.WeaponCount++;
                }, true);
            }
        }
    }
}
