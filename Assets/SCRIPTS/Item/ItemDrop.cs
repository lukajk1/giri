using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDrop : MonoBehaviour
{
    private Transform playerTransform;
    private float attractionRange = 1.8f;
    private Coroutine coroutine;
    void Start()
    {
        playerTransform = GameState.Instance.PlayerTransform;
    }
    void Update()
    {
        if (Vector2.Distance(transform.position, playerTransform.position) < attractionRange && coroutine == null)
        {
            coroutine = StartCoroutine(MoveToPlayer());
            
        }
    }
    private float closeEnoughMargin = 0.1f;
    private float speed = 22.0f;
    IEnumerator MoveToPlayer()
    {
        while (GameState.Instance.MenusOpen > 0) yield return null;

        float totalTime = 0.0f; // Total time that has passed
        Vector2 startPosition = transform.position; // Starting position

        while (Vector2.Distance(transform.position, playerTransform.position) > closeEnoughMargin)
        {
            totalTime += Time.deltaTime * speed;
            float factor = EaseInExpo(totalTime); // Calculate the easing factor

            transform.position = Vector2.Lerp(startPosition, playerTransform.position, factor);

            if (factor >= 1) break; // Break the loop if factor is complete

            yield return new WaitForSeconds(0.01f);
        }
        GameState.Instance.Audio.PlaySound(ADFM.Sfx.ItemDropPickup);
        FindObjectOfType<ItemPickupMenuManager>().Open();
        Destroy(gameObject);
    }
    float EaseInExpo(float x)
    {
        return x == 0 ? 0 : Mathf.Pow(2, 10 * (x - 1));
    }
}
