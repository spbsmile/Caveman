namespace Caveman.Bonuses
{
    public class VacuumCleanerBonus : BonusBase
    {
        public void Start()
        {
            Config = EnterPoint.Configs.Bonus["vacuumCleaner"];
        }

        //todo implement
    }
}