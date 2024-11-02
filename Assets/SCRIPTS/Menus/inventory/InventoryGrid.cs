using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryGrid : MonoBehaviour
{
    [SerializeField] private GameObject itemFrame;
    private List<(ItemData, int)> itemList;
    [SerializeField] private GameObject hotbarParent;
    [SerializeField] private InventoryManager inventoryManager;
    public static InventoryGrid Instance;
    void Awake()
    {
        if (Instance == null) Instance = this;
        else Debug.LogError($"Warning - more than one instance of {this} found. Additional occurring on {gameObject.name}");
    }
    void Start()
    {
        itemList = GameState.Instance.InventoryManager.Inventory;    
    }
    public void GenerateInventoryGrid()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
        itemList = GameState.Instance.InventoryManager.Inventory;
        foreach ((ItemData, int) item in itemList)
        {   
            GameObject obj = Instantiate(itemFrame);
            obj.transform.SetParent(transform, true);
            obj.GetComponent<InventorySlot>().SetItem(item);
        }

        SetHotbarItems();
    }

    private void SetHotbarItems()
    {
        int i = 0;
        foreach (Transform child in hotbarParent.transform)
        {
            child.GetComponent<InventorySlot>().SetItem((inventoryManager.GetItemFromHotbar(i), 1));
            i++;
        }
    }
}
