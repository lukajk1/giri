using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SpriteRenderer))]
public class MoveClickEffect : MonoBehaviour
{
    public float duration = 1f;

    private SpriteRenderer spriteRenderer;
    private Vector3 originalScale;
    private float originalAlpha;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalScale = transform.localScale;
        originalAlpha = spriteRenderer.color.a;
    }

    private void OnEnable()
    {
        StartCoroutine(AnimateEffect());
    }

    private IEnumerator AnimateEffect()
    {
        float elapsedTime = 0f;
        Color color = spriteRenderer.color;

        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;

            // Lerp scale
            transform.localScale = Vector3.Lerp(Vector3.zero, originalScale, t);

            // Lerp alpha
            color.a = Mathf.Lerp(originalAlpha, 0f, t);
            spriteRenderer.color = color;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure final values are set
        transform.localScale = originalScale;
        Destroy(gameObject);
    }
}