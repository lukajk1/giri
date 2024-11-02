using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IAPurify : ItemActivatable
{
    public override bool Activate()
    {
        GameState.Instance.Player.Cleanse();
        return true;
    }
}
