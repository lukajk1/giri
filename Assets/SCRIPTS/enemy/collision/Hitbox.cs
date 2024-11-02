using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitbox : MonoBehaviour {
    private AttackScriptable attackData;
    public void Initialize(AttackScriptable attackData)
    {
        this.attackData = attackData;
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        //Debug.Log("this should call twice");
        //Debug.Log(other.name);
        Unit unit;
        if (other.transform.root.GetComponent<Unit>())
        {
            unit = other.transform.root.GetComponent<Unit>();
        }
        else
        {
            unit = null;
        }
        if (unit != null && other.tag == "BodyHitbox") 
        {
            unit.TakeDamage(new Attack(attackData.AttackType, unit, attackData.Damage, false, attackData.StatusEffects));
        }
    }
}

