namespace Caveman.Weapons
{
    public class AxeModel : WeaponModelBase
    {
        public void Awake()
        {
            Config = EnterPoint.Configs.Weapon["axe"];
        }

        // TODO разные кривые траекторий
        public void Update()
        {
            MoveUpdate();
        }
    }
}
