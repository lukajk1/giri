using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bleed : StatusEffect, IDebuff
{
    private float intervalLength = 1.25f;
    private int procs = 4;
    private float initialDamage = 40f;
    protected override IEnumerator Timer()
    {
        GameState state = GameState.Instance;
        for (int i = 0; i < procs; i++)
        {
            yield return new WaitForSeconds(intervalLength);
            while (state.MenusOpen > 0) yield return null;
            initialDamage = Mathf.Floor(initialDamage/2);
            target.TakeDamage(new Attack(Attack.Type.StatusEffect, target, initialDamage)); 
        }
        Clear();
    }
}
