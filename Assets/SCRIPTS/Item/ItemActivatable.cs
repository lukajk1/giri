using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public abstract class ItemActivatable : MonoBehaviour
{
    protected Cooldown cooldown;
    private ItemData item;
    public ItemData Item => item;
    public Image CooldownWipe;

    protected PlayerUnit player;
    public abstract bool Activate();

    protected virtual void Start()
    {
        player = FindObjectOfType<PlayerUnit>();
    }
    protected Vector3 GetCursorWorldCoords()
    {
        Vector3 screenPos = Input.mousePosition;
        screenPos.z = Mathf.Abs(Camera.main.transform.position.z); // Set this for orthographic camera
        return Camera.main.ScreenToWorldPoint(screenPos);
    }

    protected EnemyUnit ReturnClosestEnemyToCursor(float radius=99)
    {
        EnemyUnit enemyUnit = null;
        enemyUnit = GameState.Instance.EnemySpawner.DetermineClosestTargetToCursor(radius).GetComponent<EnemyUnit>();

        if (enemyUnit != null) { 
            return enemyUnit;
        }
        else
        {
            return null;
        }
    }
}
