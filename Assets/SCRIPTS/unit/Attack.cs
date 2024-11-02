using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack
{
    private float damage;
    public float Damage
    {
        get
        {
            return damage;
        }
        set { 
            if (value < 0)
            {
                return;
            }
            else
            {
                damage = value;
            }
        }
    }

    public enum Type
    {
        AutoAttack=1,
        Skillshot=2,
        AOEField=3,
        StatusEffect=4,
        PassiveEffect=5, 
        Console=6
    }

    public Type _AttackType { get; private set; }
    public Unit Target { get; private set; }

    public bool IsCrit { get; private set; }

    public List<Unit.STFX> statusEffectList;

    public Attack(Type attackType, Unit target, float damage, bool isCrit = false, params Unit.STFX[] statusEffects)
    {
        _AttackType = attackType;
        Damage = damage;
        Target = target;

        IsCrit = isCrit;

        statusEffectList = new List<Unit.STFX>(); // initialize the list so even if params statusEffects is empty the list still exists

        foreach (Unit.STFX _stfx in statusEffects)
        {
            statusEffectList.Add(_stfx);
        }
    }
}
