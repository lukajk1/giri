using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]

[CreateAssetMenu(fileName = "NewCharacter", menuName = "CustomObjects/Character")]
public class PlayerData : UnitData
{
    [Header("Player-Specific")]
    public Canvas HealthbarCanvas;
    [Range(0, 1)] public float BaseCritChance; // 0 to 1 max
    [Range(0, 2)] public float BaseCritDamage; // 1.25 = crits deal 125% damage
    [Range(0, 4)] public float BaseAttackSpeed; // attacks per second
}
