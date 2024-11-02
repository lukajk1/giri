using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingMotion : MonoBehaviour
{
    public float bobbingSpeed;
    public float bobbingHeight;
    private GameState gameState;
    private Vector3 localStartPosition;

    private float phaseOffset;

    void Start()
    {
        gameState = GameState.Instance;
        localStartPosition = transform.localPosition;
        phaseOffset = Random.Range(0f, 2f * Mathf.PI);  // Add a random phase offset
        StartCoroutine(BobbingCoroutine());
    }

    IEnumerator BobbingCoroutine()
    {
        while (true) {
            while (gameState.MenusOpen > 0) { //pause while in menus
                yield return null;
            }
            float newY = localStartPosition.y + Mathf.Sin(Time.time * bobbingSpeed + phaseOffset) * bobbingHeight;
            transform.localPosition = new Vector3(localStartPosition.x, newY, localStartPosition.z);
            yield return null;
        }
    }

}