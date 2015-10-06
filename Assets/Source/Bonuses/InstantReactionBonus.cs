namespace Caveman.Bonuses
{
    public class InstantReactionBonus : BonusBase
    {
        public void Start()
        {
            Specification = EnterPoint.CurrentSettings.DictionaryBonuses["instantReaction"];
        }

        //todo implement
    }
}
