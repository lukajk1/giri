using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EventManager;

public class IPExecutioner : MonoBehaviour
{
    void Start()
    {
        EventManager.OnUnitTookDamage += CheckExecute;
    }
    private float executionThreshold = 0.06f;
    private void CheckExecute(Unit unit)
    {
        //executionThreshold = 0.5f; // just for testing purposes

        if (unit.CurrentHealth / unit.MaxHealth < executionThreshold)
        {
            unit.Kill(true);
            GameState.Instance.Audio.PlaySound(ADFM.Sfx.CDUp);
        }
    }
    void OnDestroy()
    {
        EventManager.OnUnitTookDamage -= CheckExecute;
    }
}
