using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UnitController : MonoBehaviour
{
    public virtual float GetCooldownStatus()
    {
        return 0;
    }
}
