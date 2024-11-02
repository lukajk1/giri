using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalAutoAttack : AutoAttack
{
    [SerializeField] private GameObject projectile;
    private Transform attackOrigin;
    private bool arcDirection;
    private PlayerUnit player;
    public override void Initialize(PlayerUnit player, Transform attackOrigin)
    {
        this.player = player;
        this.attackOrigin = attackOrigin;
    }
    public override void Attack(EnemyUnit target, float damage, bool isCrit)
    {
        GameState.Instance.Audio.PlaySound(ADFM.Sfx.PlayerAttack);
        GameObject bullet = Instantiate(projectile, attackOrigin.position, Quaternion.identity);
        bullet.GetComponent<NormalAACast>().stats = player;
        bullet.SetActive(true); 
        arcDirection = !arcDirection;
        if (target != null)
        {
            bullet.GetComponent<NormalAACast>().ShootAt(target, damage, isCrit, arcDirection);
        }
    }
}
