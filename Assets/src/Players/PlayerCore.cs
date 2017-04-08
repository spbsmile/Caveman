using System;
using Caveman.Bonuses;
using Caveman.BonusSystem;
using Caveman.Configs;

namespace Caveman.Players
{
    public partial class PlayerCore : ISupportBonus
    {
        public Action<int> WeaponCountChange;
        public Action<int> KillCountChange;
        public Action<bool> IsAliveChange;
        public Action<BonusType, float> BonusActivate;

        public Action<Action, bool> ChestActivate;

        private int weaponCount;
        private int killCount;
        private bool isAlive = true;

        public PlayerCore(string name, string id, PlayerConfig config)
        {
            Name = name;
            Id = id;
            Config = config;
            Speed = Config.Speed;
        }

        public PlayerConfig Config { get; }
        public string Id { get; }
        public string Name { get; }
        public int DeathCount { set; get; }
        public float Speed { get; private set; }

        public bool Invulnerability { get; set; }

        public int WeaponCount
        {
            get { return weaponCount; }
            set
            {
                if (weaponCount == value) return;
                weaponCount = value;
                WeaponCountChange?.Invoke(value);
            }
        }

        public int KillCount
        {
            get { return killCount; }
            set
            {
                if (killCount == value) return;
                killCount = value;
                KillCountChange?.Invoke(value);
            }
        }

        public bool IsAlive
        {
            set
            {
                if (isAlive == value) return;
                isAlive = value;
                IsAliveChange?.Invoke(value);
            }
        }
    }
}
