using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public class UnitData : ScriptableObject
{
    [Header("Universal Unit Data")]
    public string unitCommandName;
    [Multiline] public string unitVanityName;
    public bool FlyingUnit;
    public float baseMaxHealth;
    public float Shield;
    [Range(0, 18)]
    public float baseAttackRange;
    [Range(0, 999)]
    public float baseDamage;
    [Range(0, 5)]
    public float baseMoveSpeed; 
    [Range(0, 1)]
    public float baseLifesteal;
    [Range(0, 1)]
    public float baseCooldownLength = 1f;
}
