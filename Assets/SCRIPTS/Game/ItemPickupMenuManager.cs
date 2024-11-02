using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickupMenuManager : HeatData
{
    [SerializeField] private InventoryManager inventoryManager;
    [SerializeField] private GameObject itemPickupMenu;
    [SerializeField] private GameObject itemChoicesParent;
    [SerializeField] private GameObject itemChoicePrefab;

    private int itemChoicesNum = 3;
    private void Start()
    {
        itemPickupMenu.SetActive(false);
    }
    public void Open()
    {
        itemPickupMenu.SetActive(true);
        GameState.Instance.MenusOpen++;
        for (int i = 0; i < itemChoicesNum; i++)
        {
            ItemData itemData = GetRandomItem();
            if (itemData != null)
            {
                GameObject choice = Instantiate(itemChoicePrefab, itemChoicesParent.transform);
                choice.GetComponent<ItemChoicePrefab>().Initiate(itemData);
            }
            else
            {
                Debug.LogError("random item for item choice could not be found");
            }
        }
    }

    private ItemData GetRandomItem()
    {
        return inventoryManager.GetRandomItemFromItemPool();
    }

    public void Close()
    {
        foreach (Transform child in itemChoicesParent.transform)
        {
            Destroy(child.gameObject);
        }
        itemPickupMenu.SetActive(false);
        GameState.Instance.MenusOpen--;
    }
}
