using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NileDagger : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    private Transform target;
    private Transform attackOrigin;

    private const float PROJECTILE_RANGE = 7f;
    private const float PROJECTILE_SPEED = 12f;
    private const float DASH_DISTANCE = 1.4f;
    private const float DASH_SPEED = 8f;

    private float damage = 0f;

    public void Initialize(Vector3 targetDirection, PlayerUnit player)
    {
        attackOrigin = player.AttackOriginMarker;

        damage = player.Damage;

        RotateZAxis(attackOrigin.position, targetDirection);
        StartCoroutine(CastTrajectory(targetDirection));


        Vector3 projectileDirection = (targetDirection - attackOrigin.position).normalized;

        //Debug.Log(projectileDirection);
        Vector3 invertedDashDirection = new Vector3(-projectileDirection.x, -projectileDirection.y, 0);
        invertedDashDirection = invertedDashDirection.normalized * DASH_DISTANCE;

        GameState.Instance.PlayerController.Dash(player.transform.position + invertedDashDirection, DASH_SPEED);
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


    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            Unit hit = other.GetComponentInParent<Unit>();
            hit.TakeDamage(new Attack(Attack.Type.Skillshot, hit, damage, false, Unit.STFX.Speared));

            Destroy(gameObject);
        }
    }
}
