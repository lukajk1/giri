using System.Collections;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    private Vector3 originalPosition;

    private float intervalLength = 0.03f;
    private int bounces = 4;

    private PlayerUnit player;
    private CameraController cameraController; // Reference to CameraController

    void Start()
    {
        player = FindFirstObjectByType<PlayerUnit>();
        player.HealthUpdated += Shake;
        player.ShieldUpdated += Shake;

        cameraController = FindObjectOfType<CameraController>(); // Find the CameraController
    }

    private void Shake(bool isLowered)
    {
        if (isLowered) StartCoroutine(ScreenShake(.07f, .05f, .1f));
    }

    public IEnumerator ScreenShake(float xStrength, float yStrength, float duration)
    {
        GameState state = GameState.Instance;

        for (int i = 0; i < bounces; i++)
        {

            yield return new WaitForSeconds(intervalLength);
            if (state.MenusOpen > 0) yield return null;

            // Calculate the shake offset
            float xOffset = Random.Range(-1f, 1f) * xStrength;
            float yOffset = Random.Range(-1f, 1f) * yStrength;
            Vector3 shakeOffset = new Vector3(xOffset, yOffset, 0);

            // Set the shake offset in the CameraController
            cameraController.SetShakeOffset(shakeOffset);
        }

        // Reset the shake offset to zero after the shaking ends
        cameraController.SetShakeOffset(Vector3.zero);
    }
}
