using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

[CreateAssetMenu(fileName = "NewAttack", menuName = "CustomObjects/Attack")]
public class AttackScriptable : ScriptableObject
{
    [SerializeField] public Attack.Type AttackType;
    [SerializeField] public float Damage;
    [SerializeField] public Unit.STFX[] StatusEffects;

    [Range(0.1f, 20f)] public float WindupLength;
    [Range(0.1f, 99f)] public float Cooldown;

    //public Attack Initialize()
    //{
    //    return new Attack(AttackType, null, Damage, false, StatusEffects);
    //}
}
