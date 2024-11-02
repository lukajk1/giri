using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IATargetedDash : ItemActivatable
{
    public override bool Activate()
    {
        GameObject closestToCursor = GameState.Instance.EnemySpawner.DetermineClosestTargetToCursor();
        GameState.Instance.Player.GetComponentInChildren<PlayerController>().Dash(closestToCursor.transform.position, 28f);
        return true;
    }
    private Vector3 GetCursorWorldPosition()
    {
        Vector3 screenPos = Input.mousePosition;
        screenPos.z = Mathf.Abs(Camera.main.transform.position.z); // Set this for orthographic camera
        return Camera.main.ScreenToWorldPoint(screenPos);
    }
}
