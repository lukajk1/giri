using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class Unit : MonoBehaviour
{
    [HideInInspector] public float Level { get; protected set; } = 1f;
    protected float moveSpeed;
    [HideInInspector] public float MoveSpeed => moveSpeed; 
    protected float maxHealth;
    [HideInInspector] public float MaxHealth => maxHealth;
    protected float currentHealth;
    [HideInInspector] public float CurrentHealth {
        get
        {
            return currentHealth;
        }
        set
        {
            bool loweredHealth;
            if (value < currentHealth) { 
                loweredHealth = true;
            }
            else
            {
                loweredHealth = false;
            }
            currentHealth = Mathf.Floor(value);
            if (currentHealth >= MaxHealth) // overheal, could be taken advantage of by something... 
            {
                currentHealth = MaxHealth;
            }
            HealthUpdated?.Invoke(loweredHealth);
            if (currentHealth <= 0)
            {
                Kill();
            }
        }
    }

    protected float shield;
    [HideInInspector] public float Shield
    {
        get => shield;
        set
        {
            if (value <= 0) // <= so that the text still triggers if it just exactly breaks the shield
            {
                shield = 0;
                ShieldUpdated?.Invoke(true);
                damageNumbersManager.CreateMessage("shield break!", transform.position);
                GameState.Instance.Audio.PlaySound(ADFM.Sfx.ShieldBreak);
            }
            else
            {
                bool isLowered;
                if (value < shield)
                {
                    isLowered = true;   
                }
                else
                {
                    isLowered = false;
                }
                shield = Mathf.Floor(value); 
                ShieldUpdated?.Invoke(isLowered);
            }
        }
    }
    protected float damage; 
    [HideInInspector] public float Damage {
        get
        {
            return damage;
        }
        set
        {
            damage = value; // whatever it'll be funnier if you can do negative damage and handling 0 damage is more annoying
        }
    }
    protected float attackRange; [HideInInspector] public float AttackRange => attackRange; // in Unity units
    [HideInInspector] public float CooldownLength { get; protected set; } // 1 = full cooldown length, 0.5 = 50% shorter cooldowns
    [HideInInspector] public float Lifesteal { get; protected set; } // 1.25 = 125% of damage is applied as healing
    [HideInInspector] public bool IsTargetable { get; protected set; }
    [HideInInspector] public bool FlyingUnit { get; protected set; }
    [HideInInspector] public bool IsDisplayVersion { get; protected set; }
    public enum STFX
    {
        Bleed=1, Regeneration=2, Daze=3, Root=4, Stasis=5, Undamageable=7, Steadfast=8, Speared=9
    }    
    public enum Status
    {
        Default,
        Stasis,
        Dazed,
        Rooted
    }
    private Status unitStatus;
    public enum Stat
    {
        MoveSpeed,
        Lifesteal,
        Damage,
        MaxHealth,
        AttackRange,
        CooldownLength, 
        Shield
    }
    [HideInInspector] public Status UnitStatus { 
        get
        {
            return unitStatus;
        }
        set
        {
            if (value != UnitStatus)
            {
                unitStatus = value;
                StatusUpdated?.Invoke();
            }
        }
    }

    public event Action<bool> HealthUpdated;
    public event Action StatusUpdated;
    public event Action StatsUpdated;
    public event Action<bool> ShieldUpdated;
    public event Action Critted;

    protected InventoryManager inventoryManager;
    [HideInInspector] public List<Stat> StatMods = new List<Stat>();
    protected Damageable damageable;
    private DamageNumbersManager damageNumbersManager;

    //protected SpriteRenderer model; 
    private UnitEffects unitEffects;
    [HideInInspector] public bool isDying = false;

    protected void Setup(SpriteRenderer model)
    {
        //this.model = model;
        inventoryManager = GameState.Instance.InventoryManager;
        unitEffects = model.gameObject.AddComponent<UnitEffects>();
        unitEffects.Initialize(this, model);

        damageable = gameObject.AddComponent<Damageable>();
        damageable.Initialize(this, unitEffects, inventoryManager);

        damageNumbersManager = DamageNumbersManager.Instance;
    }
    public void SetPosition(Vector3 position)
    {
        transform.position = position;
    }
    public virtual void TakeDamage(Attack attack)
    {
        if (attack.IsCrit) Critted?.Invoke();
        damageable.TakeDamage(attack);
    }

    public virtual void Kill(bool executed = false)
    {
        damageable?.Kill(executed);
    }

    public void ApplySTFX(STFX effect, float duration = 99f)
    {

        if (effect == STFX.Bleed)
        {
            gameObject.AddComponent<Bleed>().Initialize(this, 0);
        }        
        else if (effect == STFX.Regeneration)
        {
            gameObject.AddComponent<Regeneration>().Initialize(this, 4f);
        }
        else if (effect == STFX.Speared)
        {
            Speared[] spearedList = gameObject.GetComponents<Speared>();
            foreach (Speared speared in spearedList)
            {
                speared.Clear();
            }

            gameObject.AddComponent<Speared>().Initialize(this, 5f);
            
        }


        else if (effect == STFX.Daze || effect == STFX.Root || effect == STFX.Stasis)
        {
            if (CheckForSTFX<Steadfast>())
            {
                damageNumbersManager.CreateMessage("immune!", transform.position);
            }
            else
            {
                if (effect == STFX.Daze)
                {
                    gameObject.AddComponent<Daze>().Initialize(this, 2.5f);
                    UnitStatus = Status.Dazed;
                    damageNumbersManager.CreateMessage("dazed!", transform.position);
                }
                else if (effect == STFX.Root)
                {
                    gameObject.AddComponent<Root>().Initialize(this, 1.5f);
                    UnitStatus = Status.Rooted;
                    damageNumbersManager.CreateMessage("rooted!", transform.position);
                }
                else if (effect == STFX.Stasis)
                {
                    gameObject.AddComponent<Stasis>().Initialize(this, 2f);
                    UnitStatus = Status.Stasis;
                    damageNumbersManager.CreateMessage("stasis!", transform.position);
                }
            }
        }
        else if (effect == STFX.Undamageable)
        {
            gameObject.AddComponent<Undamageable>().Initialize(this, 4f);
        }
        else
        {
            Debug.LogWarning($"status effect not found");
            return;
        }

        StatusUpdated?.Invoke();
    }
    public void ApplySTFX(bool isDebuff, Stat stat, float strength, float duration, bool isIndefinite = false) // method overload for stat modifiers
    {
        StatMod statMod;

        if (!isDebuff)
        {
            statMod = gameObject.AddComponent<StatMod>();
        }
        else
        {
            statMod = gameObject.AddComponent<DebuffStatMod>();
        }

        if (!isIndefinite) 
        {
            statMod.Initialize(this, stat, strength, duration);
        }
        else
        {
            statMod.Initialize(false, this, stat, strength);
        }
    }

    public bool CheckForSTFX<T>() where T : ISTFX
    {
        ISTFX[] effects = GetComponents<ISTFX>();
        foreach (ISTFX effect in effects) 
        { 
            if (effect is T)
            {
                return true;
            }
        }
        return false;
    }

    public void ModifyStat(Stat stat, float strength)
    {
        bool modified = true;

        switch (stat)
        {
            case Stat.MoveSpeed:
                moveSpeed += strength;
                break;
            case Stat.AttackRange:
                attackRange += strength;
                break;
            case Stat.Lifesteal:
                Lifesteal += strength;
                break;
            case Stat.Damage:
                Damage += strength;
                break;
            case Stat.CooldownLength:
                CooldownLength += strength;
                break;
            case Stat.Shield:
                Shield += strength;
                break;
            default:
                Debug.LogWarning("no matching stat found");
                modified = false;
                break;
        }
        //StatMods.Add(new Stat(damage: damage, strength));


        if (modified && this is PlayerUnit) StatsUpdated?.Invoke();
    }
    public void ClearedEffect()
    {
        StatusUpdated?.Invoke();
        //Debug.Log("cleared");
    }

    public void Purge()
    {
        unitEffects.Purge();

        ISTFX[] effects = gameObject.GetComponents<ISTFX>();
        if (effects.Length > 0)
        {
            damageNumbersManager.CreateMessage("purged!", transform.position);
        }
        foreach (ISTFX effect in effects) {
            effect.Clear();
        }
    }

    public void Cleanse()
    {
        unitEffects.Cleanse();

        IDebuff[] effects = gameObject.GetComponents<IDebuff>();
        if (effects.Length > 0) 
        {
            damageNumbersManager.CreateMessage("cleansed!", transform.position);
        }
        foreach (IDebuff effect in effects)
        {
            effect.Clear();
        }
    }
}
