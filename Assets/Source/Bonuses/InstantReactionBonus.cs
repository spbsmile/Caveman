namespace Caveman.Bonuses
{
    public class InstantReactionBonus : BonusBase
    {
        public void Start()
        {
            Config = EnterPoint.CurrentSettings.DictionaryBonuses["instantReaction"];
        }

        //todo implement
    }
}
