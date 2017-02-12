using Caveman.Bonuses;

namespace Caveman.BonusesSystem.Bonuses
{
    public class ShieldBonus : BonusBase
    {
        public void Start()
        {
            Config = EnterPoint.Configs.Bonus["shield"];
        }

        //todo implement
    }
}
