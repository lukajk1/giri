using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemyDamageModifier
{
    float ReturnAmountOfDamageToBeAddedToTotal(Unit stats, float damage);
}
