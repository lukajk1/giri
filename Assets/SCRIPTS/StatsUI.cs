using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StatsUI : MonoBehaviour
{
    [SerializeField] private GameObject statsParent;
    [SerializeField] private GameObject statLabel;
    [SerializeField] private Color modifiedColor;

    private PlayerUnit stats;

    private void Start()
    {
        stats = GameState.Instance.Player;
        stats.StatsUpdated += RefreshStats;
        RefreshStats();
    }
    //void OnEnable()
    //{
    //    RefreshStats();
    //}
    private void RefreshStats()
    {
        if (statsParent != null) ClearStats();

        CreateLabel("health", stats.MaxHealth, stats.MaxHealth != stats.Data.baseMaxHealth);

        CreateLabel("move speed", $"{stats.MoveSpeed.ToString("F1")} units/sec", stats.MoveSpeed != stats.Data.baseMoveSpeed);

        CreateLabel("damage", stats.Damage, stats.Damage != stats.Data.baseDamage);

        CreateLabel("attack range", $"{stats.AttackRange.ToString("F1")} units", stats.AttackRange != stats.Data.baseAttackRange);

        CreateLabel("attack speed", $"{stats.AttackSpeed.ToString("F1")} attacks/sec", stats.AttackSpeed != stats.Data.BaseAttackSpeed);

        CreateLabel("cooldowns length", $"{(stats.CooldownLength * 100).ToString("F0")}%", stats.CooldownLength != stats.Data.baseCooldownLength);

        CreateLabel("crit chance", $"{(stats.CritChance * 100).ToString("F0")}%", stats.CritChance != stats.Data.BaseCritChance);

        CreateLabel("crit damage", $"{(stats.CritDamage * 100).ToString("F0")}%", stats.CritDamage != stats.Data.BaseCritDamage);
    }

    GameObject statInstance;
    private void CreateLabel(string label, float value, bool modifiedFromBase)
    {
        statInstance = Instantiate(statLabel, statsParent.transform); 
        statInstance.GetComponent<TextMeshProUGUI>().text = label;
        statInstance.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = value.ToString();
        if ( modifiedFromBase ) statInstance.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = modifiedColor;
    }

    private void CreateLabel(string label, string value, bool modifiedFromBase)
    {
        statInstance = Instantiate(statLabel, statsParent.transform);
        statInstance.GetComponent<TextMeshProUGUI>().text = label; 
        statInstance.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = value;
        if (modifiedFromBase) statInstance.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = modifiedColor;
    }

    private void ClearStats()
    {
        foreach (Transform child in statsParent.transform)
        {
            Destroy(child.gameObject);
        }
    }

    //void OnDisable()
    //{   
    //    ClearStats();
    //}
}
