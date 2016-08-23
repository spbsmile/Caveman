using System;
using Caveman.Configs;
using UnityEditor;

namespace Caveman.Players
{
    public class PlayerCore 
    {
        public Action<int> WeaponCountChange;
        public Action<int> KillCountChange;
	    public Action<bool> IsAliveChange;
	    public Action<BonusConfig.Types, float> BonusActivate;
       
        private int weaponCount;
        private int killCount;
	    private bool isAlive;
	    private float speed;

	    public PlayerCore(string name, string id, PlayerConfig config)
        {
            Name = name;
            Id = id;
	        Config = config;
	        speed = Config.Speed;
        }

	    public PlayerConfig Config { get; private set; }
	    public string Id { get; private set; }
        public string Name { get; private set; }
	    public int DeathCount { set; get; }

	    public float Speed
	    {
		    get { return speed; }
		    set { speed = value; }
	    }

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

	    //todo very strange
        public void ActivatedBonus(BonusConfig.Types type, float duration)
        {
            if (BonusActivate != null)
            {
                BonusActivate(type, duration);
            }
        }
    }
}
