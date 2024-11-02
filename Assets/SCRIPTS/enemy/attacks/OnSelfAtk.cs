using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnSelfAtk : MonoBehaviour, IEnemyAttack
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
    private GameObject atk;
    //private bool attacked;
    public void Attack(AttackScriptable attackData)
    {
        atk = Instantiate(_data.attackPrefab, transform.position, Quaternion.identity, _obj.transform);
        atk.GetComponent<FieldWindup>().Initialize(attackData);
        atk.GetComponent<Hitbox>().Initialize(attackData);
        //attacked = true;
    }
    void Update()
    {
        //if (attacked) gameObject.GetComponent<EnemyController>().SetState(EnemyController.State.Attacking);
    }
}
