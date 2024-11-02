using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInstance : MonoBehaviour
{
    private int stackCount; public int StackCount => stackCount;
    private ItemData item; public ItemData Item => item;
    private float moveSpeed; public float MoveSpeed => moveSpeed;

    private float health; public float Health => health;

    private float damage; public float Damage => damage;

    private float attackRange; public float AttackRange => attackRange;

    private float attackSpeed; public float AttackSpeed => attackSpeed;

    private float cooldownReduction; public float CooldownReduction => cooldownReduction;

    private float critChance; public float CritChance => critChance;

    private float critDamage; public float CritDamage => critDamage;

    private float lifesteal; public float Lifesteal => lifesteal;
    private float cooldownLength; public float CooldownLength => cooldownLength;

    public void AddOneToStackCount()
    {
        stackCount++;
    }
    public void Initialize(ItemData itemData)
    {
        item = itemData;
        //moveSpeed += item.MoveSpeed;
        //damage += item.Damage;
        //attackRange += item.AttackRange;
        //attackSpeed += item.AttackSpeed;
        //cooldownLength += item.CooldownReduction;
        //critChance += item.CritChance;
        //critDamage += item.CritDamage;
    }
}
