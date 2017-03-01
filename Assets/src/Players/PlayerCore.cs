using System;
using Caveman.Bonuses;
using Caveman.BonusSystem;
using Caveman.Configs;

namespace Caveman.Players
{
    public class PlayerCore : ISupportBonus
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

	    public PlayerConfig Config { get; private set; }
	    public string Id { get; private set; }
        public string Name { get; private set; }
	    public int DeathCount { set; get; }

	    public float Speed { get; private set; }

        public int WeaponCount
        {
            get { return weaponCount; }
            set
            {
	            if (weaponCount == value) return;
	            weaponCount = value;
	            if (WeaponCountChange != null)
	            {
		            WeaponCountChange(value);
	            }
            }
        }

        public int KillCount
        {
            get { return killCount; }
            set
            {
	            if (killCount == value) return;
	            killCount = value;
	            if (KillCountChange != null)
	            {
		            KillCountChange(value);
	            }
            }
        }

        public bool Invulnerability { get; set; }

        public bool IsAlive
	    {
		    get { return isAlive; }
		    set
		    {
			    if (isAlive == value) return;
			    isAlive = value;
			    if (IsAliveChange != null)
			    {
                    IsAliveChange(value);
			    }
		    }
	    }

        public void ActivatedBonus(BonusType type, float duration)
        {
            if (BonusActivate != null)
            {
                BonusActivate(type, duration);
            }
        }

        public void ActivatedChest(Action openHandler, bool isOpenGui)
        {
            if (ChestActivate != null)
            {
                ChestActivate(openHandler, isOpenGui);
            }
        }

        public void ChangeSpeed(float factor)
        {
            Speed = factor > 0 ? Speed*factor : Speed/factor*(-1);
        }
    }
}
