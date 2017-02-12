using System.Collections;
using Caveman.Bonuses;
using UnityEngine;

namespace Caveman.BonusesSystem.Bonuses
{
    public class SpeedBonus : BonusBase
    {
        public void Awake()
        {
            Config = EnterPoint.Configs.Bonus["speed"];
        }

        public override void Effect(ISupportBonus item)
        {
            base.Effect(item);
            item.ChangeSpeed(Config.Factor);
        }

        protected override IEnumerator UnEffect(ISupportBonus item)
        {
            yield return new WaitForSeconds(Config.Duration);
            pool.Store(this);
            item.ChangeSpeed(-Config.Factor);
        }
    }
}
