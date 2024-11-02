using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedOnPlayerAtk : MonoBehaviour, IEnemyAttack
{
    private EnemyData _data;
    private GameObject _obj;
    public GameObject parentEnemyObject
    {
        set { _obj = value; }
    }

    public EnemyData data
    {
        set { _data = value; }
    }
    public void Attack(AttackScriptable attackData)
    {
        GameObject atk = Instantiate(_data.attackPrefab, GameState.Instance.PlayerTransform.position, Quaternion.identity);

        atk.GetComponent<Hitbox>().Initialize(attackData);
    }

}
