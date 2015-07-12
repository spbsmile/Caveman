namespace Caveman.Weapons
{
    public class SkullModel : WeaponModelBase
    {
        public override WeaponType Type
        {
            get { return WeaponType.Skull; }
        }
      
        // TODO разные кривые траекторий
        public void Update()
        {
            MotionUpdate();
        }
    }
}
