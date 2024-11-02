using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Image itemIcon;
    private GameObject tooltipObject;
    private Tooltip tooltip;
    [SerializeField] private TextMeshProUGUI stackText;
    [HideInInspector] public ItemData ItemData { get; set; }
    //public bool toolTipCanOpen = true;
    void Start() {
        tooltipObject = FindObjectOfType<ItemManager>().Tooltip;
        tooltip = tooltipObject.GetComponent<Tooltip>();
        GetComponent<RectTransform>().localScale = Vector3.one;
        //itemIcon.enabled = false;
    }

    public void SetItem((ItemData, int) item)
    {
        if (item.Item1 != null)
        {
            ItemData = item.Item1;
            itemIcon.sprite = item.Item1.Icon;
            itemIcon.enabled = true;
            if (item.Item2 > 1) // stack size must be greater than 1 to show a number
            {
                stackText.text = item.Item2.ToString();
            }
            else
            {
                stackText.text = "";
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (eventData.pointerEnter == gameObject && ItemData != null)
        {
            //tooltipObject.GetComponent<RectTransform>().anchoredPosition = gameObject.GetComponent<RectTransform>().anchoredPosition;
            tooltip.Open(ItemData);
            GameState.Instance.Audio.PlaySound(ADFM.Sfx.ButtonHover);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // Check if the pointer has exited this object, not a child
        if (eventData.pointerEnter == gameObject) {
            tooltip.Exit();
        }
    }
}
