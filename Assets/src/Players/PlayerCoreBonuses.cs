using System;
using Caveman.BonusSystem;

namespace Caveman.Players
{
    public partial class PlayerCore
    {
        public void ActivatedBonus(BonusType type, float duration)
        {
            BonusActivate?.Invoke(type, duration);
        }

        public void ActivatedChest(Action openHandler, bool isOpenGui)
        {
            ChestActivate?.Invoke(openHandler, isOpenGui);
        }

        public void ChangeSpeed(float factor)
        {
            Speed = factor > 0 ? Speed * factor : Speed / factor * -1;
        }
    }
}