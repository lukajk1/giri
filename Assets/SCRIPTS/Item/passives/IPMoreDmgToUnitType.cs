using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class IPMoreDmgToUnitType : MonoBehaviour, IEnemyDamageModifier
{
    private float percentage = 0;
    private bool targetingFlyingUnits;
    public void Initiate(bool shouldTargetFlying, float percent)
    {
        targetingFlyingUnits = shouldTargetFlying;
        percentage = percent;
    }
    public float ReturnAmountOfDamageToBeAddedToTotal(Unit stats, float damage)
    {
        if (targetingFlyingUnits && stats.FlyingUnit)
        {
            return damage * percentage;
        }
        else if (!targetingFlyingUnits && !stats.FlyingUnit)
        {
            return damage * percentage;
        }
        else
        {
            return 0;
        }
    }
}
