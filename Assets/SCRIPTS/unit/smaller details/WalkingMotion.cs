using System.Collections;
using UnityEngine;

public class WalkingMotion : MonoBehaviour
{
    private EnemyData data;
    private float baseWalkingSpeed;
    private float speedMultiplier;
    private float stepHeight;
    private float rotationAngle;

    private Vector3 localStartPosition;
    private bool walkingEnabled;
    public bool WalkingEnabled 
    { 
        get
        {
            return walkingEnabled;
        } 
        set
        {
            walkingEnabled = value;
            SetWalking(value);
        }
    }

    private Coroutine walkingCoroutine;  // Field to store reference to the coroutine

    public void Initialize(EnemyData data)
    {
        this.data = data;
        baseWalkingSpeed = data.baseWalkingSpeed;
        speedMultiplier = data.speedMultiplier;
        stepHeight = data.stepHeight;
        rotationAngle = data.rotationAngle;
        localStartPosition = transform.localPosition;
    }


    private void SetWalking(bool value)
    {
        if (value && walkingCoroutine == null)
        {
            walkingCoroutine = StartCoroutine(WalkingCoroutine());
        }
        else if (!value && walkingCoroutine != null)
        {
            StopCoroutine(walkingCoroutine);
            walkingCoroutine = null;
            ResetPositionAndRotation();
        }
    }

    IEnumerator WalkingCoroutine()
    {
        float randomOffset = Random.Range(0.0f, 1.5f);
        float startTime = Time.time + randomOffset;

        while (true)
        {
            while (GameState.Instance.MenusOpen > 0)  // Pause while menus are open
            {
                yield return null;
            }

            float cycleTime = (Time.time - startTime) * baseWalkingSpeed;
            float sinWave = Mathf.Sin(cycleTime);
            float cosWave = Mathf.Cos(cycleTime);

            // Dynamically adjust walking speed based on sine wave proximity to zero
            float currentSpeed = baseWalkingSpeed * (1 + (speedMultiplier * Mathf.Clamp01(1f - Mathf.Abs(sinWave))));

            // Adjust position to simulate walking
            float newY = localStartPosition.y + Mathf.Abs(sinWave) * stepHeight;
            transform.localPosition = new Vector3(localStartPosition.x, newY, localStartPosition.z);

            // Calculate rotation
            float newRotationZ = sinWave * rotationAngle;
            transform.localRotation = Quaternion.Euler(0, 0, newRotationZ);


            yield return null;
        }
    }

    private void ResetPositionAndRotation()
    {
        transform.localPosition = localStartPosition;
        transform.localRotation = Quaternion.identity;
    }
}
