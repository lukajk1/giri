using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

[CreateAssetMenu(fileName = "NewEnemy", menuName = "CustomObjects/Enemy")]
public class EnemyData : UnitData
{

    public enum AttackType
    {
        Multi = 1,
        RangedCaster = 2,
        RangedOnPlayer = 3,
        AimedMelee = 4,
        OnSelf = 5,
        Kamikaze = 6
    }
    [Header("Enemy-Specific")]
    public AttackType AttackStyle;
    public AttackScriptable attackData;

    [Header("attack specs")]
    [Range(-3, 0)] public float fieldOffsetDistance;

    [Header("assets")]
    public GameObject enemyPrefab;
    public GameObject attackPrefab;
    public AudioClip attackSFX;
    public string attackSFXName;

    [Header("bobbing motion")]
    public float bobbingSpeed;
    public float bobbingHeight;

    [Header("walking motion")]
    public float baseWalkingSpeed;  // Base speed of the walking cycle
    public float speedMultiplier;   // Multiplier to increase speed at direction changes
    public float stepHeight;       // Vertical height of each step
    public float rotationAngle;    // Maximum rotation angle for stepping

    [Header("for multiattack")]
    [Range(0, 4)]
    public float intervalBetweenAttacks;
    [Range(0, 10)]
    public int numberOfAttacks;
    public bool randomRotation;

}
