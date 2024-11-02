using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IAPercentageShield : ItemActivatable
{
    public override bool Activate()
    {
        PlayerUnit player = GameState.Instance.Player;
        player.ApplySTFX(false, Unit.Stat.Shield, 0.3f * player.MaxHealth, 3f);
        return true;
    }
}
