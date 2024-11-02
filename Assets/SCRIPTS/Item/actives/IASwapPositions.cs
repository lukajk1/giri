using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IASwapPositions : ItemActivatable
{
    public override bool Activate()
    {
        GameObject closestToCursor = GameState.Instance.EnemySpawner.DetermineClosestTargetToCursor();
        Vector3 enemyPos = closestToCursor.transform.position;
        closestToCursor.GetComponent<Unit>().SetPosition(GameState.Instance.PlayerTransform.position);
        GameState.Instance.Player.SetPosition(enemyPos);
        GameState.Instance.PlayerController.SetTargetMovementPositionTo(enemyPos);
        return true;
    }
}
