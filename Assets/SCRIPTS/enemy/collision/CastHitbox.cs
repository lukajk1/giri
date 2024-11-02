using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastHitbox : MonoBehaviour
{
    private AttackScriptable attackData;
    public void Initialize(AttackScriptable attackData)
    {
        this.attackData = attackData;
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.root.GetComponent<PlayerUnit>() && other.CompareTag("BodyHitbox"))
        {
            other.transform.root.GetComponent<PlayerUnit>().TakeDamage(
                new Attack(attackData.AttackType, other.transform.root.GetComponent<PlayerUnit>(), attackData.Damage, false, attackData.StatusEffects)
                );

            //other.transform.root.GetComponent<PlayerUnit>().ApplySTFX(true, Unit.Stat.Damage, -50f, 3f);
            Destroy(gameObject);
        }
    }
}

