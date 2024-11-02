using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedCasterAtk : MonoBehaviour, IEnemyAttack
{
    public EnemyData data {  get; set; }
    private GameObject _obj;
    public GameObject parentEnemyObject
    {
        set { _obj = value; }
    }
    public void Initialize(EnemyData data)
    {
        this.data = data;
    }
    public void Attack(AttackScriptable attackData)
    {
        GameObject atk = Instantiate(data.attackPrefab, transform.position, Quaternion.identity);
        atk.GetComponent<CastingAttackInstance>().Initialize(attackData, data);
        if (data.attackSFXName != null)
        {
            GameState.Instance.Audio.PlaySound(ADFM.Sfx.EnemyCastAttack);
        }
    }
}
