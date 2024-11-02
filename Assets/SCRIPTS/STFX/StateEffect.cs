using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateEffect : StatusEffect
{
    public override void Clear()
    {
        target.UnitStatus = Unit.Status.Default;
        target.ClearedEffect();
        //Debug.Log("clearing with the stateeffect clear");
        Destroy(this);
    }
}
