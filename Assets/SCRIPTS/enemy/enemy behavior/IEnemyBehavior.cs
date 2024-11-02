using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemyBehavior
{
    void Initialize(EnemyUnit unit, EnemyData data);
}
