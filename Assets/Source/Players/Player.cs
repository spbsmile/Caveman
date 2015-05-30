using System;

namespace Caveman.Players
{
    public class Player
    {
        public Action<int> WeaponsCountChanged;
        public Action<int> KillsCountChanged;

        public int deaths;
        public readonly string name;
        //todo hack
        public float countRespawnThrow = 1;
       
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
    }
}
