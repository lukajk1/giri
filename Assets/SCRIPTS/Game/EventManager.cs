using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    // Define a delegate and event for enemy kills
    public delegate void EnemyKilled(EnemyUnit enemy);
    public static event EnemyKilled OnEnemyKilled;

    // Method to invoke the OnEnemyKilled event
    public static void EnemyKilledEvent(EnemyUnit enemy)
    {
        OnEnemyKilled?.Invoke(enemy);
    }

    public delegate void UnitTookDamage(Unit unit);
    public static event UnitTookDamage OnUnitTookDamage;
    public static void UnitTookDamageEvent(Unit unit)
    {
        OnUnitTookDamage?.Invoke(unit);
    }
}
