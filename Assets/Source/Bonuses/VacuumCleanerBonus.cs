namespace Caveman.Bonuses
{
    public class VacuumCleanerBonus : BonusBase
    {
        public void Start()
        {
            Config = EnterPoint.CurrentSettings.DictionaryBonuses["vacuumCleaner"];
        }

        //todo implement
    }
}