using System.Collections;
using Caveman.Bonuses;
using Caveman.Players;
using Caveman.Setting;
using UnityEngine;

public class SpeedBonus : BonusBase 
{
    public override void Effect(PlayerModelBase playerModel)
    {
        if (playerModel.bonusBase != null) return;
        base.Effect(playerModel);
        preValue = playerModel.Speed;
        playerModel.Speed = playerModel.Speed*2;
    }

    protected override IEnumerator UnEffect(PlayerModelBase playerModel)
    {
        yield return new WaitForSeconds(Settings.BonusSpeedDuration);
        pool.Store(this);
        playerModel.bonusBase = null;
        playerModel.Speed = preValue;
    }
}
