namespace Caveman.Weapons
{
    public class AxeModel : WeaponModelBase
    {
        public void Awake()
        {
            Config = EnterPoint.CurrentSettings.DictionaryWeapons["axe"];
        }

        // TODO разные кривые траекторий
        public void Update()
        {
            MotionUpdate();
        }
    }
}
