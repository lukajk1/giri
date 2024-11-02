using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameAlert : MonoBehaviour
{
    private TextMeshProUGUI myText;
    private RectTransform rectTransform;
    public enum Reason
    {
        CooldownNotUp,
        NotInRange
    }
    public void Initiate(Reason reason)
    {
        myText = gameObject.GetComponent<TextMeshProUGUI>();
        rectTransform = gameObject.GetComponent<RectTransform>();

        switch (reason)
        {
            case Reason.CooldownNotUp:
                myText.text = "active is on cooldown!";
                break;
            case Reason.NotInRange:
                myText.text = "no target in range!";
                break;
        }

        StartCoroutine(FadeAndRise());
    }
    private IEnumerator FadeAndRise()
    {
        float duration = 2.4f; // Duration of the animation in seconds
        float rate = 60f; // Speed of the rise movement
        float fadeRate = 1.5f / duration; // Rate of fading

        float elapsed = 0;
        Vector3 startPosition = rectTransform.anchoredPosition;
        Color startColor = myText.color;

        while (elapsed < duration)
        {
            while (GameState.Instance.MenusOpen > 0) yield return null;
            float increment = Time.deltaTime;
            rectTransform.anchoredPosition += new Vector2(0, rate * increment);

            myText.color = new Color(startColor.r, startColor.g, startColor.b, Mathf.Clamp(myText.color.a - fadeRate * increment, 0, 1));

            elapsed += increment;
            yield return null; // Wait until the next frame
        }

        myText.color = new Color(startColor.r, startColor.g, startColor.b, 0); // Ensure the text is fully transparent after animation
        Destroy(gameObject);
    }
}
