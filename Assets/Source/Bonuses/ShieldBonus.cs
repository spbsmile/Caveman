namespace Caveman.Bonuses
{
    public class ShieldBonus : BonusBase
    {
        public void Start()
        {
            Config = EnterPoint.CurrentSettings.BonusesConfigs["shield"];
        }

        //todo implement
    }
}
