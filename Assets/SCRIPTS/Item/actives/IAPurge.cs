using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IAPurge : ItemActivatable
{
    public override bool Activate()
    {
        GameState.Instance.Player.Purge();
        return true;
    }
}
