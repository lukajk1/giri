using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;




#if UNITY_EDITOR
using UnityEditor;
#endif

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }

    public List<(ItemData, int)> Inventory = new List<(ItemData, int)>(); // player inventory 
    [HideInInspector] private List<ItemData> itemMasterList = new List<ItemData>(); 
    [HideInInspector] private List<ItemData> availablePoolOfItemsInGame = new List<ItemData>(); 

    [SerializeField] private ItemData[] hotbar = new ItemData[6];
    public int HotbarSize = 6;
    void Start()
    {
        GenerateMasterList();
        availablePoolOfItemsInGame = itemMasterList;
    }

    public void Equip(ItemData item)
    {
        if (item == null)
        {
            Debug.LogError("inventorymanager method equip() called with null ItemData argument");
            return;
        }
        else if (!availablePoolOfItemsInGame.Contains(item))
        {
            Debug.LogError($"attempted to equip unique item {item.CommandItemName} when item has been removed from available pool");
            return;
        }
        Inventory.Add((item, 1));
        BuildStatsFromInventory();
        if (!String.IsNullOrWhiteSpace(item.Passive) && !String.IsNullOrEmpty(item.Passive))
        {
            switch (item.Passive)
            {
                case "IPDoubleAllDamage":
                    gameObject.AddComponent<IPModDmgToAllUnits>().Initiate(1f);
                    break;
                case "IPExecutioner":
                    AddItemPassiveComponentIfNotAlreadyPresent<IPExecutioner>();
                    break;
                case "IPMoreDmgToGrounded":
                    IPMoreDmgToUnitType typeGrounded = gameObject.AddComponent<IPMoreDmgToUnitType>();
                    typeGrounded.Initiate(false, 0.09f);
                    break;
                case "IPMoreDmgToFlying":
                    IPMoreDmgToUnitType typeFlying = gameObject.AddComponent<IPMoreDmgToUnitType>();
                    typeFlying.Initiate(true, 0.08f);
                    break;
                default:
                    Debug.LogError($"could not find corresponding passive script for passive {item.Passive}");
                    break;
            }
        }

        if (item.IsUnique || !String.IsNullOrWhiteSpace(item.Active)) // each item with an active can only be obtained once so you don't have repeats in the hotbar, too much to keep track of
        {
            availablePoolOfItemsInGame.Remove(item);
        }
        EnsureAllInventoryItemsAreInStacks();
    }
    private void AddItemPassiveComponentIfNotAlreadyPresent<T>() where T : Component
    {
        if (gameObject.GetComponent<T>() == null)
        {
            gameObject.AddComponent<T>();
        }
    }
    public float ModifyDamageToUnits(Unit stats, float damage) // returns total amount of modified damage, does not include the base damage of the attack
    {
        float modifiedDamage = 0;
        IUnitDamageModifier[] modifiers = GetComponents<IUnitDamageModifier>();
        foreach (IUnitDamageModifier modifier in modifiers)
        {
            modifiedDamage += modifier.ReturnAmountOfDamageToBeAddedToTotal(stats, damage);
        }
        return modifiedDamage;
    }
    public float ModifyDamageToEnemies(Unit stats, float damage) // returns total amount of modified damage, does not include the base damage of the attack
    {
        float modifiedDamage = 0;
        IEnemyDamageModifier[] modifiers = GetComponents<IEnemyDamageModifier>();
        foreach (IEnemyDamageModifier modifier in modifiers)
        {
            modifiedDamage += modifier.ReturnAmountOfDamageToBeAddedToTotal(stats, damage);
        }
        return modifiedDamage; 
    }

    public void BuildStatsFromInventory()
    {
        PlayerUnit charStats = FindObjectOfType<PlayerUnit>();
        charStats.SetStatsToBase(); //reset before rebuilding
        // set flat stats first
        foreach ((ItemData, int) item in Inventory)
        {
            charStats.AddFlatStats(item.Item1, item.Item2);
        }
        foreach (ItemData item in hotbar) 
        {
            if (item != null)
            {
                charStats.AddFlatStats(item);
            }
        }
    }

    private void GenerateMasterList()
    {
        // Load all Item assets from the Resources folder
        ItemData[] items = Resources.LoadAll<ItemData>("Items");
        foreach (ItemData item in items)
        {
            if (item != null)
            {
                itemMasterList.Add(item);
            }
        }
    }
    public void ItemDropChoice(ItemData item)
    {
        if (!String.IsNullOrWhiteSpace(item.Active))
        {
            FindObjectOfType<ReplaceHotbarOrSendToInventory>().Open(item);
        }
        else
        {
            Equip(item);
        }
    }

    public ItemData[] GetHotbar()
    {
        return hotbar;
    }

    public ItemData GetRandomItemFromItemPool()
    {
        int index = UnityEngine.Random.Range(0, availablePoolOfItemsInGame.Count);
        return availablePoolOfItemsInGame[index];
    }

    public ItemData GetItemFromCommandName(string name)
    {
        string lowerCaseName = name.ToLowerInvariant();
        foreach (ItemData item in itemMasterList)
        {
            if (lowerCaseName == item.CommandItemName.ToLowerInvariant())
            {
                return item;
            }
        }
        Debug.LogError($"could not find item with name {name}");
        return null;
    }

    public void SetItemToHotbar(ItemData item, int index)
    {
        if (item == null)
        {
            Debug.LogError($"attempted to set null item to hotbar");
            return;
        }
        else if (String.IsNullOrWhiteSpace(item.Active))
        {
            Debug.LogError($"canot set {item.VanityItemName} to hotbar - does not have an active");
            return;
        }

        
        if (index >= 0 && index < hotbar.Length)
        {
            if (hotbar[index] != null) // if there's already an item at that hotbar index, send that item to the inventory first
            {
                Inventory.Add((hotbar[index],1));
                EnsureAllInventoryItemsAreInStacks();
            }
            hotbar[index] = item;
            FindObjectOfType<HotbarManager>().SetSlot(index, item);
            BuildStatsFromInventory(); 
            availablePoolOfItemsInGame.Remove(item);
        }
        else
        {
            Debug.LogError("Index out of range");
        }
    }
    private void EnsureAllInventoryItemsAreInStacks()
    {
        var itemDictionary = new Dictionary<ItemData, int>();

        foreach (var item in Inventory)
        {
            ItemData itemData = item.Item1;
            int quantity = item.Item2;

            if (itemDictionary.ContainsKey(itemData))
            {
                itemDictionary[itemData] += quantity;
            }
            else
            {
                itemDictionary[itemData] = quantity;
            }
        }

        Inventory.Clear();  // Clear the original list

        foreach (var kvp in itemDictionary)
        {
            Inventory.Add((kvp.Key, kvp.Value));  // Re-populate the list with merged entries
        }
    }

    public ItemData GetItemFromHotbar(int index)
    {
        if (index >= 0 && index < hotbar.Length)
        {
            return hotbar[index];
        }
        else
        {
            Debug.LogError("Index out of range");
            return null;
        }
    }
    void Awake() {
        if (Instance == null) Instance = this; 
        else Debug.LogError($"Warning - more than one instance of {this} found. Additional occurring on {gameObject}"); 
    }
}
