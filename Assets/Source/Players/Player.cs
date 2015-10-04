using System;
using Caveman.Specification;

namespace Caveman.Players
{
    public class Player
    {
        public Action<int> WeaponsCountChanged;
        public Action<int> KillsCountChanged;
        public Action<BonusSpecification.Types, float> BonusActivated;
        public int deaths;
        public readonly string name;
        private int weapons;
        private int kills;

        public Player(string name)
        {
            this.name = name;
        }

        public int Weapons
        {
            get { return weapons; }
            set
            {
                if (weapons != value)
                {
                    weapons = value;
                    if (WeaponsCountChanged != null)
                    {
                        WeaponsCountChanged(weapons);
                    }
                }
            }
        }

        public int Kills
        {
            get { return kills; }
            set
            {
                if (kills != value)
                {
                    kills = value;
                    if (KillsCountChanged != null)
                    {
                        KillsCountChanged(kills);
                    }
                }
            }
        }

        public void PickUpBonus(BonusSpecification.Types type, float duration)
        {
            if (BonusActivated != null)
            {
                BonusActivated(type, duration);
            }
        }
    }
}
