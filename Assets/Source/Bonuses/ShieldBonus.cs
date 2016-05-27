namespace Caveman.Bonuses
{
    public class ShieldBonus : BonusBase
    {
        public void Start()
        {
            Config = EnterPoint.CurrentSettings.DictionaryBonuses["shield"];
        }

        //todo implement
    }
}
