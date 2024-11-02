using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemyAttack
{
    EnemyData data { set; }
    GameObject parentEnemyObject { set; }
    void Attack(AttackScriptable attackData);
}
