using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IPModDmgToAllUnits : MonoBehaviour, IUnitDamageModifier
{
    private float percentage = 0;
    public void Initiate(float _percent)
    {
        percentage = _percent;
    }
    public float ReturnAmountOfDamageToBeAddedToTotal(Unit stats, float damage)
    {
        return damage * percentage;
    }
}
