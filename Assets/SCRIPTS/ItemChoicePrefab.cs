using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ItemChoicePrefab : MonoBehaviour
{
    [SerializeField] private Tooltip tooltip;
    [SerializeField] private Image itemIcon;
    private ItemData itemData;
    public void Initiate(ItemData data)
    {
        itemData = data;
        tooltip.Open(data);
        itemIcon.sprite = data.Icon;
    }

    public void Clicked()
    {
        FindObjectOfType<InventoryManager>().ItemDropChoice(itemData);
        FindObjectOfType<ItemPickupMenuManager>().Close();
    }
}
