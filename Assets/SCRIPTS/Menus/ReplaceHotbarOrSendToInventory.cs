using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ReplaceHotbarOrSendToInventory : MonoBehaviour
{
    [SerializeField] private GameObject replaceHotbarMenu;
    [SerializeField] private TMP_Dropdown hotbarDropdown;
    [SerializeField] private Button sendToInventory;
    [SerializeField] private Button confirmHotbarSlotPos;
    private HotbarManager hotbarManager;
    private InventoryManager inventoryManager;
    private ItemData itemInQuestion;

    void Start()
    {
        replaceHotbarMenu.SetActive(false);
        hotbarManager = FindObjectOfType<HotbarManager>();
        inventoryManager = FindObjectOfType<InventoryManager>();

        confirmHotbarSlotPos.onClick.AddListener(SendToHotbarSlot);
        sendToInventory.onClick.AddListener(SendToInventory);
    }
    public void Open(ItemData item)
    {
        GameState.Instance.MenusOpen++;
        replaceHotbarMenu.SetActive(true);
        itemInQuestion = item;
    }
    private void SendToHotbarSlot()
    {
        inventoryManager.SetItemToHotbar(itemInQuestion, hotbarDropdown.value);
        Close();
    }

    private void SendToInventory()
    {
        inventoryManager.Equip(itemInQuestion);
        Close();
    }

    public void Close()
    {
        GameState.Instance.MenusOpen--;
        replaceHotbarMenu.SetActive(false);
    }

}
