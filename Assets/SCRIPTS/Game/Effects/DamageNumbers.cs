using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamageNumbers : MonoBehaviour
{

    private float fadeDuration = 0.8f;
    private float height;
    private TMP_Text dmgText;
    private Color startingColor;


    public void Initialize(float damage, bool isCrit)
    {
        PrivateSetup();

        if (isCrit)
        {
            startingColor = dmgText.color;
            dmgText.text = $"*{damage.ToString()}";
        }
        else
        {
            startingColor = Color.white;
            dmgText.text = damage.ToString();
        }

        StartCoroutine(FadeOutText());
    }    
    
    public void Initialize(string message)
    {
        PrivateSetup();

        startingColor = Color.white;
        dmgText.text = message;
        StartCoroutine(FadeOutText());
    }

    private void PrivateSetup()
    {
        height = Random.Range(8f, 12f);
        dmgText = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
    }

    private IEnumerator FadeOutText() {
        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        Vector2 startPosition = rectTransform.anchoredPosition += new Vector2(0, 0f);
        float elapsedTime = 0f;
        float xOffset = Random.Range(-1, 1f);

        while (elapsedTime < fadeDuration) {
            while (GameState.Instance.MenusOpen > 0) yield return null;

            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(1, 0, elapsedTime / fadeDuration);
            dmgText.color = new Color(startingColor.r, startingColor.g, startingColor.b, alpha);

            float x = Mathf.Lerp(startPosition.x, startPosition.x + xOffset, elapsedTime / fadeDuration); // Horizontal movement (adjust as needed)
            float y = startPosition.y + height * (4 * elapsedTime / fadeDuration * (1 - elapsedTime / fadeDuration)); // Parabolic movement

            dmgText.rectTransform.anchoredPosition = new Vector2(x, y);
            yield return null;
        }
        Destroy(gameObject);
    }
}
