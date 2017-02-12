using Caveman.BonusSystem;

namespace Caveman.Bonuses
{
    public interface ISupportBonus
    {
        void ChangeSpeed(float factor);

        void ActivatedBonus(BonusType type, float duration);
    }
}