using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IABlink : ItemActivatable
{
    private float maxDistance = 5.0f;
    //public ParticleSystem blinkParticles;

    public override bool Activate()
    {
        Vector3 cursorPos = GetCursorWorldPosition();

        float distanceToCursor = Vector3.Distance(GameState.Instance.PlayerTransform.position, cursorPos);
        if (distanceToCursor <= maxDistance)
        {
            GameState.Instance.PlayerRootObject.transform.position = cursorPos;
        }
        else
        {
            Vector3 direction = (cursorPos - GameState.Instance.PlayerTransform.position).normalized;
            GameState.Instance.PlayerRootObject.transform.position += direction * maxDistance;
        }

        GameState.Instance.Audio.PlaySound(ADFM.Sfx.Blink);
        //blinkParticles.Play();
        GameState.Instance.PlayerRootObject.GetComponentInChildren<PlayerController>().SetTargetPosition(GameState.Instance.PlayerRootObject.transform.position);

        return true;
    }

    private Vector3 GetCursorWorldPosition()
    {
        Vector3 screenPos = Input.mousePosition;
        screenPos.z = Mathf.Abs(Camera.main.transform.position.z); // Set this for orthographic camera
        return Camera.main.ScreenToWorldPoint(screenPos);
    }
}
