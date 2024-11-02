using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IAStasis : ItemActivatable
{
    public override bool Activate()
    {
        GameState.Instance.Player.ApplySTFX(Unit.STFX.Stasis);
        return true;
    }
}
