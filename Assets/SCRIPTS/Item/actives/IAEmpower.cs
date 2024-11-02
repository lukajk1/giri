using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IAEmpower : ItemActivatable
{
    public override bool Activate()
    {
        Unit player = GameState.Instance.Player;

        player.ApplySTFX(isDebuff:false, Unit.Stat.MoveSpeed, strength:2f, duration:6f);
        player.ApplySTFX(isDebuff:false, Unit.Stat.AttackRange, strength:1f, duration:6f);
        player.ApplySTFX(isDebuff:false, Unit.Stat.Damage, strength:30f, duration:6f);

        return true;
    }
}
