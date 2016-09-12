namespace Caveman.Bonuses
{
    public class InstantReactionBonus : BonusBase
    {
        public void Start()
        {
            Config = EnterPoint.CurrentSettings.BonusesConfigs["instantReaction"];
        }

        //todo implement
    }
}
