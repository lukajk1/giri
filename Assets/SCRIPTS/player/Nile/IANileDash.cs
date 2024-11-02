using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IANileDash : ItemActivatable
{
    private const float DASH_DISTANCE = 1.2f;
    private const float DASH_SPEED = 8f;

    public override bool Activate()
    {
        EnemyUnit enemy = ReturnClosestEnemyToCursor();

        if (enemy != null) {
            if (enemy.CheckForSTFX<Speared>())
            {
                Vector3 dir = (enemy.transform.position - player.transform.position).normalized * DASH_DISTANCE;

                GameState.Instance.PlayerController.Dash(player.transform.position + dir, DASH_SPEED);
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }
}
