using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiFieldAtk : MonoBehaviour, IEnemyAttack
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
        StartCoroutine(AttackSequence(attackData));
    }

    private IEnumerator AttackSequence(AttackScriptable attackData)
    {
        for (int i = 0; i < _data.numberOfAttacks; i++)
        {
            Quaternion rotation = _data.randomRotation ? RandomRotation() : Quaternion.identity;
            GameObject atk = Instantiate(_data.attackPrefab, GameState.Instance.PlayerTransform.position, rotation);

            atk.GetComponent<Hitbox>().Initialize(attackData);

            yield return new WaitForSeconds(_data.intervalBetweenAttacks);
        }
    }


    private Quaternion RandomRotation()
    {
        float angle = Random.Range(0, 360);
        return Quaternion.Euler(0, 0, angle);
    }
}
