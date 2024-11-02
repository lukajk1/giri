using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PlayerHurtEffect : MonoBehaviour
{
    // Start is called before the first frame update
    private PlayerUnit player;
    [SerializeField] private Material hurtEffect;
    private bool isRunning;
    [SerializeField] private ScriptableRendererFeature fullscreenEffect;

    private int _power = Shader.PropertyToID("_Power");

    void Start()
    {
        //fullscreenEffect.SetActive(false);
        player = FindFirstObjectByType<PlayerUnit>();
        player.HealthUpdated += Effect;
        hurtEffect.SetFloat(_power, 7f);
    }

    private void Effect(bool loweredHealth)
    {
        if (loweredHealth && !isRunning) {
            StartCoroutine(LerpHurtEffect(1.1f));
        }
    }

    private IEnumerator LerpHurtEffect(float duration)
    {
        isRunning = true;
        fullscreenEffect.SetActive(true);
        float startValue = 5.5f;
        float endValue = 2.2f;
        float elapsedTime = 0f;
        GameState state = GameState.Instance;

        hurtEffect.SetFloat(_power, endValue);

        while (elapsedTime < duration)
        {
            while (state.MenusOpen > 0) yield return null;
            hurtEffect.SetFloat(_power, Mathf.Lerp(endValue, startValue, elapsedTime / duration));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        hurtEffect.SetFloat(_power, startValue);
        isRunning = false;
        fullscreenEffect.SetActive(false);
    }

}
