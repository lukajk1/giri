using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitEffects : MonoBehaviour
{
    private Unit unit;
    private SpriteRenderer model;
    private GameAssets assets;
    private Material defaultMaterial;
    private Coroutine runningCoroutine;
    public void Initialize(Unit unit, SpriteRenderer model)
    {
        assets = GameAssets.Instance;

        this.model = model;
        this.unit = unit;

        defaultMaterial = model.material;

        unit.StatusUpdated += DetermineEffect;
    }

    private void DetermineEffect()
    {
        switch (unit.UnitStatus)
        {
            case Unit.Status.Dazed:
                Dazed();
                break;
            case Unit.Status.Rooted:
                Rooted();
                break;
            case Unit.Status.Stasis:
                Stasis();
                break;
            default:
                break;
        }
    }
    private void CheckForRunningCoroutine(IEnumerator newCoroutine)
    {
        if (runningCoroutine != null)
        {
            StopCoroutine(runningCoroutine);
        }

        runningCoroutine = StartCoroutine(newCoroutine);
    }

    public void Damaged()
    {
        Material material = new Material(assets.Damaged);
        CheckForRunningCoroutine(SwapMaterials(material, 0.13f));
    }
    private IEnumerator SwapMaterials(Material material, float duration)
    {
        model.material = material;
        float elapsedTime = 0f;
        GameState state = GameState.Instance;

        while (elapsedTime < duration)
        {
            while (state.MenusOpen > 0) yield return null;
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        model.material = defaultMaterial;
        runningCoroutine = null;
    }

    public void Cleanse()
    {
        Material material = new Material(assets.Cleanse);
        CheckForRunningCoroutine(Dissolve(material));
    }

    private IEnumerator Dissolve(Material material)
    {
        model.material = material;
        float duration = 0.4f;
        float elapsedTime = 0f;
        GameState state = GameState.Instance;

        while (elapsedTime < duration)
        {
            while (state.MenusOpen > 0) yield return null;

            material.SetFloat("_DissolveAmount", Mathf.Lerp(0.2f, 1.1f, elapsedTime / duration));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        material.SetFloat("_DissolveAmount", 1.1f);  // Ensure final value is set
        model.material = defaultMaterial;
        runningCoroutine = null;
    }


    public void Purge()
    {
        Material material = new Material(assets.Purge);
        CheckForRunningCoroutine(Dissolve(material));
    }

    public void Dazed()
    {
        CheckForRunningCoroutine(SwapMaterials(new Material(assets.Dazed), 2.5f));
    }    
    
    public void Rooted()
    {
        CheckForRunningCoroutine(SwapMaterials(new Material(assets.Rooted), 1.5f));
    }
    
    public void Stasis()
    {
        CheckForRunningCoroutine(SwapMaterials(new Material(assets.Stasis), 2f));
    }
}
