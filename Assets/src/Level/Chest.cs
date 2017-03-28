using System;
using Caveman.Players;
using UnityEngine;

namespace Caveman.Level
{
    public class Chest : MonoBehaviour
    {
        public Action openChest;

        public Sprite chestOpenIcon;

        private SpriteRenderer render;

        private State currentState;

        private PlayerCore owner;

        private enum State
        {
            Close,
            Open
        }

        public void Start()
        {
            render = GetComponent<SpriteRenderer>();
            openChest = Open;
            currentState = State.Close;
        }

        public void OnTriggerEnter2D(Collider2D other)
        {
            if (currentState == State.Close && other.GetComponent<PlayerModelHero>())
            {
                owner = other.GetComponent<PlayerModelHero>().PlayerCore;
                owner.ActivatedChest(openChest, true);
            }
        }

        public void  OnTriggerExit2D(Collider2D other)
        {
            if (currentState == State.Close && other.GetComponent<PlayerModelHero>())
            {
                owner.ActivatedChest(openChest, false);
                owner = null;
            }
        }

        private void Open()
        {
            currentState = State.Open;
            render.sprite = chestOpenIcon;
            owner.WeaponCount++;
        }
    }
}
