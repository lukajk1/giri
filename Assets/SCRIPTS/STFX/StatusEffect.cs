using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StatusEffect : MonoBehaviour, ISTFX
{
    protected float duration;
    protected Unit target;
    public virtual void Initialize(Unit target, float duration)
    {
        this.target = target;
        this.duration = duration;
        StartCoroutine(Timer());
    }
    public virtual void Initialize(bool isIndefinite, Unit target) // alternate initialization for indefinite status effects. Boolean does not do anything but forces me to explicitly acknowledge I'm applying an indefinite statuseffect
    {
        this.target = target;
    }

    protected virtual IEnumerator Timer()
    {
        float elapsedTime = 0f;
        GameState state = GameState.Instance;
        while (elapsedTime < duration) 
        {
            if (state.MenusOpen > 0) yield return null;
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        Clear();
    }

    public virtual void Clear()
    {
        target.ClearedEffect();
        Destroy(this);
    }
}
