using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class OnDeathPostProcessing : MonoBehaviour
{
    [SerializeField] private Volume volume;

    private ChromaticAberration chromaticAberration;
    private LensDistortion lensDistortion;

    private float lensDistortEndValue = -0.5f;
    private float chromaticEndValue = 1f;

    void Start()
    {
        volume.profile.TryGet(out chromaticAberration);
        volume.profile.TryGet(out lensDistortion);

        //chromaticAberration.intensity.value = 1f;
    }

    public void StartEffects()
    {
        StartCoroutine(EaseOutCubicBezierChroma(.1f, chromaticEndValue));
        StartCoroutine(EaseOutCubicBezierLens(0f, lensDistortEndValue));
    }

    private float duration = 15f;

    private IEnumerator EaseOutCubicBezierLens(float startValue, float endValue)
    {
        yield return new WaitForSeconds(1);
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / duration);

            float easedT = EaseOut(t);

            lensDistortion.intensity.value = Mathf.Lerp(startValue, endValue, easedT);

            yield return null;
        }

        lensDistortion.intensity.value = endValue;
    }    
    
    private IEnumerator EaseOutCubicBezierChroma(float startValue, float endValue)
    {
        yield return new WaitForSeconds(1);
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / duration);

            float easedT = EaseOut(t);

            chromaticAberration.intensity.value = Mathf.Lerp(startValue, endValue, easedT);

            yield return null;
        }

        chromaticAberration.intensity.value = endValue;
    }
    private float EaseOut(float x)
    {
        //return 1 - Mathf.Pow(1 - x, 5);
        return x;
    }

    public void ResetValuesForNewGame()
    {
        chromaticAberration.intensity.value = 0;
        lensDistortion.intensity.value = 0;
    }
}
