using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUnitDamageModifier
{
    float ReturnAmountOfDamageToBeAddedToTotal(Unit stats, float damage);
}
