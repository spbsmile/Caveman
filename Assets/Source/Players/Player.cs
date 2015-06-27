using System;

namespace Caveman.Players
{
    public class Player
    {
        public static int idCounter;
        public Action<int> WeaponsCountChanged;
        public Action<int> KillsCountChanged;
        public int deaths;
        public readonly string name;
        public readonly int id;

        private int weapons;
        private int kills;

        public Player(string name)
        {
            this.name = name;
            id = idCounter++;
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
