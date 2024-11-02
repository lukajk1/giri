using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyUnit : Unit
{
    private EnemyData Data;
    public float AttackCooldown;
    [HideInInspector] public SpriteRenderer model;
    public void Initialize(EnemyData data, SpriteRenderer model)
    {
        this.Data = data;
        this.model = model;
        Setup(model);

        moveSpeed = Data.baseMoveSpeed;
        maxHealth = Data.baseMaxHealth;
        currentHealth = MaxHealth;
        damage = Data.baseDamage;
        attackRange = Data.baseAttackRange;
        FlyingUnit = Data.FlyingUnit;
        Shield = Data.Shield;
    }

    public override void Kill(bool executed = false)
    {
        //GameState.Instance.Audio.PlaySound("enemyDeath");
        EventManager.EnemyKilledEvent(this);
        GameState.Instance.Audio.PlaySound(ADFM.Sfx.EnemyDeath);
        base.Kill(executed); 
    }

}
