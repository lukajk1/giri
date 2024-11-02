using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpearCast : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    private Transform target;
    private Transform attackOrigin;


    private const float PROJECTILE_RANGE = 8f;
    private const float PROJECTILE_SPEED = 22f;


    public void Initialize(EnemyUnit target, PlayerUnit player, bool isCrit)
    {
        attackOrigin = player.AttackOriginMarker;

        GetComponent<SpearHitbox>().Initialize(player.Damage, isCrit);

        Vector3 targetPosition = target.transform.position + new Vector3(0, 1.3f, 0); // this is to make the spear go through approximately the midsection of enemies. Yes I am too lazy to make this a proper value in enemies

        RotateZAxis(attackOrigin.position, targetPosition);
        StartCoroutine(CastTrajectory(targetPosition));
    }

    private IEnumerator CastTrajectory(Vector3 targetPosition)
    {
        Vector3 direction = (targetPosition - attackOrigin.position).normalized;
        GameState gs = GameState.Instance;
        float distanceTraveled = 0f;

        while (distanceTraveled < PROJECTILE_RANGE)
        {
            while (gs.MenusOpen > 0) yield return null; // Pausing while menus are open

            // Adjust movement to frame-rate independent with Time.fixedDeltaTime
            Vector2 movement = (Vector2)direction * PROJECTILE_SPEED * Time.fixedDeltaTime;
            rb.MovePosition(rb.position + movement);

            distanceTraveled += movement.magnitude;

            yield return new WaitForFixedUpdate();  // For consistency with physics
        }

        Destroy(gameObject);
    }
    void RotateZAxis(Vector3 pointA, Vector3 pointB)
    {
        // Step 1: Calculate the direction from pointA to pointB
        Vector3 direction = (pointB - pointA).normalized;

        // Step 2: Calculate the angle between the direction and the forward axis (optional, assuming a 2D plane where Z rotation matters)
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg; 
        angle -= 90f;

        // Step 3: Apply the rotation to the object so its Z axis points towards the angle
        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }
}
