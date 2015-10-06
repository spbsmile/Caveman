namespace Caveman.Bonuses
{
    public class ShieldBonus : BonusBase
    {
        public void Start()
        {
            Specification = EnterPoint.CurrentSettings.DictionaryBonuses["shield"];
        }

        //todo implement
    }
}
