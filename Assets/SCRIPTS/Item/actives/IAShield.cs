using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IAShield : ItemActivatable
{
    public override bool Activate()
    {
        GameState.Instance.Player.ApplySTFX(false, Unit.Stat.Shield, 300f, 4f);
        return true;
    }
}
