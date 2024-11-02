using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimedMeleeAtk : MonoBehaviour, IEnemyAttack
{
    private EnemyData _data;
    private GameObject _obj;
    public GameObject parentEnemyObject
    {
        set { _obj = value; }
    }

    public EnemyData data
    {
        set { _data = value; }
    }
    private GameObject atk;
    //private bool attacked;
    public void Attack(AttackScriptable attackData)
    {
        Quaternion targetRotation = GetRotationTowardsTarget(GameState.Instance.PlayerTransform.position);
        Vector3 offsetPosition = GetOffsetPosition(transform.position, targetRotation, _data.fieldOffsetDistance);

        atk = Instantiate(_data.attackPrefab, offsetPosition, targetRotation, _obj.transform);
        atk.GetComponent<FieldWindup>().Initialize(attackData);
        atk.GetComponentInChildren<Hitbox>().Initialize(attackData);
        //attacked = true;
    }
    void Update()
    {
        //if (attacked) gameObject.GetComponent<EnemyController>().SetState(EnemyController.State.Attacking);
    }

    private Quaternion GetRotationTowardsTarget(Vector3 targetPosition)
    {
        Vector3 direction = targetPosition - transform.position;

        // Calculate the angle between the forward direction and the target direction
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        angle += 180f;
        return Quaternion.Euler(new Vector3(0, 0, angle));
    }

    private Vector3 GetOffsetPosition(Vector3 currentPosition, Quaternion rotation, float distance)
    {
        // Calculate the forward direction from the quaternion
        Vector3 forwardDirection = rotation * Vector3.right;

        // Calculate the offset position
        Vector3 offsetPosition = currentPosition + forwardDirection * distance;

        return offsetPosition;
    }
}
