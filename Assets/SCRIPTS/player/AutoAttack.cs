using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AutoAttack : MonoBehaviour
{
    public virtual void Initialize(PlayerUnit player, Transform attackOrigin) { }
    public virtual void Attack(EnemyUnit target, float damage, bool isCrit) { }
}
