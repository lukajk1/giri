using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime.Misc;

public class PlayerUnit : Unit
{

    [HideInInspector] public float CritChance { get; protected set; } // 0 to 1 max
    [HideInInspector] public float CritDamage { get; protected set; } // 1.25 = crits deal 125% damage
    [HideInInspector] public float AttackSpeed { get; protected set; } // in attacks per second
    [HideInInspector] public int EXPERIENCE { get; protected set; }
    [HideInInspector] public Dictionary<string, float> StatDictionary { get; protected set; }
    [HideInInspector] public float TotalDamageDealt;
    [HideInInspector] public float TotalDamageTaken;
    [HideInInspector] public int TotalActivesUsed;

    public SpriteRenderer Model;
    public Transform AttackOriginMarker;
    public PlayerData Data;
    void Awake()
    {
        SetStatsToBase(); 
    }
    void Start()
    {
        Setup(Model);

        HB_Setup hp = gameObject.AddComponent<HB_Setup>();
        hp.Initialize(this, Data);
        
        FindObjectOfType<InventoryManager>().BuildStatsFromInventory();
    }

    public void SetStatsToBase()
    {
        moveSpeed = Data.baseMoveSpeed;
        maxHealth = Data.baseMaxHealth;
        currentHealth = MaxHealth;
        damage = Data.baseDamage;
        attackRange = Data.baseAttackRange;
        shield = Data.Shield;

        CooldownLength = Data.baseCooldownLength;
        CritChance = Data.BaseCritChance;
        CritDamage = Data.BaseCritDamage;
        AttackSpeed = Data.BaseAttackSpeed;
        FlyingUnit = Data.FlyingUnit;
    }

    public void AddFlatStats(ItemData item, int stackCount = 1)
    {
        if (item == null)
        {
            Debug.LogError("tried to add stats with null item argument");
            return;
        }
        for (int i = 0; i < stackCount; i++)
        {
            moveSpeed += item.MoveSpeed;
            maxHealth += item.Health;
            currentHealth += item.Health;
            damage += item.Damage;
            attackRange += item.AttackRange;
            AttackSpeed += item.AttackSpeed;
            CooldownLength -= item.CooldownReduction;
            CritChance += item.CritChance;
            CritDamage += item.CritDamage;
        }
    }


    public override void Kill(bool executed = false)
    {
        GameState.Instance.Audio.PlaySound(ADFM.Sfx.PlayerDeath);
        FindObjectOfType<GameOver>().GameEnd(false);
    }

}
