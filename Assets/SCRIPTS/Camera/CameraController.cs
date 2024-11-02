using UnityEngine;

public class CameraController : MonoBehaviour
{
    private bool panEnabled = true;
    [SerializeField] private KeybindMap keyMap;

    [SerializeField] private GameObject background;
    private Vector3 relativeOffset;
    private Vector3 backgroundHomePosition; // Home position for the background

    private GameState gameState;
    public bool CameraLocked;
    private Vector3 shakeOffset = Vector3.zero; // Offset for camera shake
    private Vector3 lockedPosition; // Store the locked position for applying shake
    private float panSpeed = 27f;
    private float edgeThreshold = 10f; // Distance from screen edge to trigger panning

    private void Start()
    {
        gameState = GameState.Instance;
        relativeOffset = Vector3.zero;
        backgroundHomePosition = background.transform.position; // Initialize home position
        CameraLocked = true;
    }

    void Update()
    {
        UpdateBackground();

        if ((Input.GetKey(keyMap.KeyMap["center camera"]) || CameraLocked) && gameState.MenusOpen == 0)
        {
            // Store the locked position (e.g., following the player)
            lockedPosition = gameState.PlayerTransform.position + new Vector3(0, 0, -10f);

            // Apply locked position with shakeOffset
            transform.position = lockedPosition + shakeOffset;

            ResetBackgroundPosition(); // Reset background when centering the camera
        }
        else if (panEnabled)
        {
            Vector3 move = new Vector3();
            if (Input.mousePosition.x >= Screen.width - edgeThreshold)
            {
                move.x = panSpeed * Time.deltaTime;
            }
            else if (Input.mousePosition.x <= edgeThreshold)
            {
                move.x = -panSpeed * Time.deltaTime;
            }

            if (Input.mousePosition.y >= Screen.height - edgeThreshold)
            {
                move.y = panSpeed * Time.deltaTime;
            }
            else if (Input.mousePosition.y <= edgeThreshold)
            {
                move.y = -panSpeed * Time.deltaTime;
            }

            // Translate the camera with the calculated movement
            transform.Translate(move, Space.World);

            // Apply the shake offset after the camera's movement
            transform.position += shakeOffset;
        }
        else
        {
            // If the camera isn't locked or panning, just apply the shake offset
            transform.position += shakeOffset;
        }
    }

    public void SetPanEnabled(bool enabled)
    {
        panEnabled = enabled;
    }

    private const float BGWidth = 19.2f;
    private const float BGHeight = 12.8f;
    private void UpdateBackground()
    {
        Vector3 movement = Vector3.zero;

        // Handle horizontal movement
        if (transform.position.x - relativeOffset.x >= BGWidth - 2f) // Moving right
        {
            relativeOffset.x = transform.position.x;
            movement.x = BGWidth;
        }
        else if (transform.position.x - relativeOffset.x <= -BGWidth + 2f) // Moving left
        {
            relativeOffset.x = transform.position.x;
            movement.x = -BGWidth;
        }

        // Handle vertical movement
        if (transform.position.y - relativeOffset.y >= BGHeight - 1.8f) // Moving up
        {
            relativeOffset.y = transform.position.y;
            movement.y = BGHeight;
        }
        else if (transform.position.y - relativeOffset.y <= -BGHeight + 1.8f) // Moving down
        {
            relativeOffset.y = transform.position.y;
            movement.y = -BGHeight;
        }

        // Apply the calculated movement to the background's position
        if (movement != Vector3.zero)
        {
            background.transform.position += movement;
            if (Vector2.Distance(background.transform.position, gameState.PlayerTransform.position) < 20f)
            {
                background.transform.position = backgroundHomePosition;
            }
        }
    }

    private void ResetBackgroundPosition()
    {
        background.transform.position = backgroundHomePosition; // Reset to home position
    }

    // Method to set the shake offset from the CameraShake script
    public void SetShakeOffset(Vector3 offset)
    {
        shakeOffset = offset;
    }
}
