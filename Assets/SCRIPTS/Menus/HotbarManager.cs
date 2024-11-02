using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HotbarManager : MonoBehaviour
{
    //private KeybindMap _keyMap;
    private InventoryManager inventoryManager;
    [SerializeField] private GameObject hotbarSlotParent;
    [SerializeField] private GameObject hotbarSlotPrefab;
    private ItemData[] hotbar;
    private HUDSlot[] slots = new HUDSlot[6];
    private Dictionary<string, KeyCode> keyMap;

    void Start() {
        keyMap = GameState.Instance.KeyMapInstance.KeyMap;
        inventoryManager = InventoryManager.Instance;
        hotbar = inventoryManager.GetHotbar();
        SetHotbar();
    }

    public void SetHotbar()
    {
        for (int i = 0; i < hotbar.Length; i++)
        {
            GameObject slot = Instantiate(hotbarSlotPrefab, hotbarSlotParent.transform);
            slot.GetComponent<HUDSlot>().SetSlot(hotbar[i]);
            slots[i] = slot.GetComponent<HUDSlot>();
        }
        slots[0].SetKeybindName(keyMap["slot 1"].ToString());
        slots[1].SetKeybindName(keyMap["slot 2"].ToString());
        slots[2].SetKeybindName(keyMap["slot 3"].ToString());
        slots[3].SetKeybindName(keyMap["slot 4"].ToString());
        slots[4].SetKeybindName(keyMap["slot 5"].ToString());
        slots[5].SetKeybindName(keyMap["slot 6"].ToString());
    }

    public void SetSlot(int slot, ItemData item)
    {
        slots[slot].SetSlot(item);
    }

    void Update()
    {
        if (GameState.Instance.MenusOpen == 0)
        {
            if (Input.GetKeyDown(GameState.Instance.KeyMapInstance.KeyMap["slot 1"]))
            {
                slots[0].Activate();
            }
            else if (Input.GetKeyDown(GameState.Instance.KeyMapInstance.KeyMap["slot 2"]))
            {
                slots[1].Activate();
            }
            else if (Input.GetKeyDown(GameState.Instance.KeyMapInstance.KeyMap["slot 3"]))
            {
                slots[2].Activate();
            }
            else if (Input.GetKeyDown(GameState.Instance.KeyMapInstance.KeyMap["slot 4"]))
            {
                slots[3].Activate();
            }
            else if (Input.GetKeyDown(GameState.Instance.KeyMapInstance.KeyMap["slot 5"]))
            {
                slots[4].Activate();
            }
            else if (Input.GetKeyDown(GameState.Instance.KeyMapInstance.KeyMap["slot 6"]))
            {
                slots[5].Activate();
            }
        }
    }
}
