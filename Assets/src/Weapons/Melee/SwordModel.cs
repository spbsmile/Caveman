using Caveman.Pools;

namespace Caveman.Weapons.Melee
{
    public class SwordModel : WeaponModelBase
    {
        public void Awake()
        {
            Config = EnterPoint.Configs.Weapon["sword"];
        }

        public override void InitializationPool(ObjectPool<WeaponModelBase> item)
        {
            throw new System.NotImplementedException();
        }

        public override void Destroy()
        {
            throw new System.NotImplementedException();
        }

    }
}