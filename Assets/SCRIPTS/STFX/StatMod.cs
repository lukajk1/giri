using System.Collections;
using UnityEngine;

public class StatMod : MonoBehaviour, ISTFX
{
    private float strength;
    private Unit.Stat stat;
    private bool cleared = false;
    private Unit target;
    private float duration;

    public void Initialize(Unit target, Unit.Stat stat, float strength, float duration)
    {
        this.duration = duration;
        this.target = target;
        this.strength = strength;
        this.stat = stat;

        target.ModifyStat(stat, strength);
        StartCoroutine(Timer());
    }
    public void Initialize(bool isIndefinite, Unit target, Unit.Stat stat, float strength) // indefinite variation
    {
        this.target = target;
        this.strength = strength;
        this.stat = stat;

        target.ModifyStat(stat, strength);
    }

    protected IEnumerator Timer()
    {
        GameState state = GameState.Instance;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            if (state.MenusOpen > 0) yield return null;
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        target.ModifyStat(stat, -strength);
        cleared = true;
        Clear();
    }

    public void Clear()
    {
        if (!cleared) target.ModifyStat(stat, -strength);
        target.ClearedEffect();
        Destroy(this);
    }
}
