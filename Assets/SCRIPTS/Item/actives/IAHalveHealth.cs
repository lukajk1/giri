using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class IAHalveHealth : ItemActivatable
{
    public override bool Activate()
    {
        GameObject closest = GameState.Instance.EnemySpawner.DetermineClosestTargetToCursor();

        if (closest != null && Vector2.Distance(closest.transform.position, GameState.Instance.PlayerTransform.position) < 6f) {

            //closest.GetComponent<Unit>().ModifyPercentHealth(false, 0.5f);
            Unit unit = closest.GetComponent<Unit>();
            closest.GetComponent<Unit>().TakeDamage(
                new Attack(Attack.Type.PassiveEffect, unit, unit.MaxHealth * 0.5f)
                );

            GameState.Instance.Audio.PlaySound(ADFM.Sfx.Blink);
            return true;
        }
        else
        {
            GameState.Instance.Audio.PlaySound(ADFM.Sfx.CDNotUp);
            GameAssets.Instance.Alert(GameAlert.Reason.NotInRange);
            return false;
        }
    }
}
