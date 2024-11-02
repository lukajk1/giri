using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuandryShield : MonoBehaviour, IEnemyBehavior
{
    private EnemyUnit unit;
    private EnemyData data;
    private bool spent = false;
    public void Initialize(EnemyUnit unit, EnemyData data)
    {
        this.unit = unit;
        this.data = data;
        unit.HealthUpdated += CheckForActivation;
    }

    private void CheckForActivation(bool isLowered)
    {
        if (!spent && unit.CurrentHealth <= 0.4f * unit.MaxHealth)
        {
            DamageNumbersManager.Instance.CreateMessage("procced!", transform.position);
            //unit.Shield += 500f;
            unit.ApplySTFX(false, Unit.Stat.Shield, 500f, 1.8f);
            spent = true;
        }
        else
        {
            //Destroy(this);
        }
    }
}
