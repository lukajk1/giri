using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpearHitbox : MonoBehaviour
{
    private bool firstHit = true;
    private float damage;
    private bool isCrit;

    public void Initialize(float damage, bool isCrit)
    {
        this.damage = damage;
        this.isCrit = isCrit;
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            Unit hit = other.GetComponentInParent<Unit>();
            if (firstHit) { 
                hit.TakeDamage(new Attack(Attack.Type.Skillshot, hit, damage, isCrit, Unit.STFX.Speared));
                firstHit = false;
            }
            else
            {
                hit.TakeDamage(new Attack(Attack.Type.Skillshot, hit, damage / 2, isCrit, Unit.STFX.Speared));
            }
        }
    }
}
