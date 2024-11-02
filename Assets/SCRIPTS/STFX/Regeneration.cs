using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Regeneration : StatusEffect
{
    float interval = 1f;
    float healAmount = 40f;

    protected override IEnumerator Timer()
    {
        GameState state = GameState.Instance;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            while (state.MenusOpen > 0) yield return null;
            target.CurrentHealth += healAmount;
            elapsedTime += interval;
            yield return new WaitForSeconds(interval);
        }
        Clear();
    }
}
