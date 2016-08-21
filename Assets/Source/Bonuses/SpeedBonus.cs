using System.Collections;
using Caveman.Players;
using Caveman.Setting;
using UnityEngine;

namespace Caveman.Bonuses
{
    public class SpeedBonus : BonusBase
    {
        public void Awake()
        {
            Config = EnterPoint.CurrentSettings.BonusesConfigs["speed"];
        }

        public override void Effect(PlayerModelBase model)
        {
            if (model.bonusBase != null)
                return;
            base.Effect(model);
            preValue = model.PlayerCore.Speed;
            model.PlayerCore.Speed = model.PlayerCore.Speed * 2;
        }

        protected override IEnumerator UnEffect(PlayerModelBase model)
        {
            yield return new WaitForSeconds(Settings.BonusSpeedDuration);
            pool.Store(this);
            model.bonusBase = null;
            model.PlayerCore.Speed = preValue;
        }
    }
}
