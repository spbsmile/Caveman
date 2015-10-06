namespace Caveman.Bonuses
{
    public class VacuumCleanerBonus : BonusBase
    {
        public void Start()
        {
            Specification = EnterPoint.CurrentSettings.DictionaryBonuses["vacuumCleaner"];
        }

        //todo implement
    }
}