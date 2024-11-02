using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SpearAutoAttack : AutoAttack
{
    [SerializeField] private GameObject spear;

    private Transform attackOrigin;
    private PlayerUnit player;
    public override void Initialize(PlayerUnit player, Transform attackOrigin)
    {
        this.player = player;
        this.attackOrigin = attackOrigin;
    }

    public override void Attack(EnemyUnit target, float damage, bool isCrit)
    {
        GameState.Instance.Audio.PlaySound(ADFM.Sfx.PlayerAttack);

        GameObject atk = Instantiate(spear, attackOrigin.position, Quaternion.identity);
        atk.GetComponent<SpearCast>().Initialize(target, player, isCrit);
        atk.SetActive(true);

    }
}
