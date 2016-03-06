using System;
using Caveman.Specification;

namespace Caveman.Players
{
    public class Player
    {
        // event for gui
        public Action<int> WeaponsCountChanged;
        public Action<int> KillsCountChanged;
        public Action<BonusSpecification.Types, float> BonusActivated;
       
        private int weapons;
        private int kills;

        public Player(string name, string id)
        {
            Name = name;
            Id = id;
        }

        public string Id { get; private set; }
        public string Name { get; private set; }
        public int Deaths { set; get; }

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

        //todo very strange
        public void PickUpBonus(BonusSpecification.Types type, float duration)
        {
            if (BonusActivated != null)
            {
                BonusActivated(type, duration);
            }
        }
    }
}
