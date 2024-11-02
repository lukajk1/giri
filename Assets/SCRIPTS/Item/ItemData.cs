using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

[CreateAssetMenu(fileName = "NewItem", menuName = "CustomObjects/Item")]
public class ItemData : ScriptableObject {
    [Header("Meta")]
    [SerializeField] private Sprite icon; public Sprite Icon => icon;

    [SerializeField] private string commandItemName; public string CommandItemName => commandItemName;

    [SerializeField] [Multiline] private string vanityItemName; public string VanityItemName => vanityItemName;

    [SerializeField] [TextArea(4, 20)] private string description; public string Description => description;
    [SerializeField] [TextArea(2, 20)] private string flavorText; public string FlavorText => flavorText;

    //[SerializeField] private string equipSFXName; public string EquipSFXName => equipSFXName;

    [SerializeField] private int tier; public int Tier => tier;
    [SerializeField] private bool _isUnique; public bool IsUnique => _isUnique;


    [Header("Stats")]

    [SerializeField] private float moveSpeed; public float MoveSpeed => moveSpeed;

    [SerializeField] private float health; public float Health => health;

    [SerializeField] private float damage; public float Damage => damage;

    [SerializeField] private float attackRange; public float AttackRange => attackRange;

    [SerializeField] private float attackSpeed; public float AttackSpeed => attackSpeed;

    [SerializeField] [Range(0, 1)] private float cooldownReduction; public float CooldownReduction => cooldownReduction;

    [SerializeField] [Range(0, 1)] private float critChance; public float CritChance => critChance;

    [SerializeField] [Range(0, 1)] private float critDamage; public float CritDamage => critDamage;

    [SerializeField] [Range(0, 1)] private float lifesteal; public float Lifesteal => lifesteal; 
    
    [Header("Passive")]
    //[SerializeField] private bool hasPassive; public bool HasPassive => hasPassive;
    [SerializeField] private string _passive; public string Passive => _passive;

    [Header("Active")]

    //[SerializeField] private bool hasActive; public bool HasActive => hasActive;

    [SerializeField] [Range(1, 90)] private float cooldown;  public float Cooldown => cooldown;

    [SerializeField] private string activatableEffect; public string Active => activatableEffect;


    [HideInInspector] public Dictionary<string, float> statNameDictionary;

    void OnEnable()
    {
        statNameDictionary = new Dictionary<string, float>
        {
            { "move speed", MoveSpeed },
            { "health", Health },
            { "damage", Damage },
            { "attack range", AttackRange },
            { "attack speed", AttackSpeed },
            { "cooldown reduction", CooldownReduction },
            { "crit chance", CritChance },
            { "crit damage", CritDamage }
        };
    }


}
