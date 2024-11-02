using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class DraggableUI : MonoBehaviour
    //, IPointerDownHandler, IPointerUpHandler, IDragHandler // need these
{
    //private RectTransform rectTransform;
    //private Canvas canvas;
    //private Vector2 originalLocalPointerPosition;
    //private Vector3 originalPanelLocalPosition;
    //public TabMenu tabMenu;
    //public InventorySlot onHover;

    //void Awake()
    //{
    //    rectTransform = GetComponent<RectTransform>();
    //    canvas = GetComponentInParent<Canvas>();

    //    if (canvas == null)
    //    {
    //        Debug.LogError("The UI element is not part of a Canvas");
    //    }
    //}

    //public void OnPointerDown(PointerEventData eventData)
    //{
    //    originalPanelLocalPosition = rectTransform.anchoredPosition;
    //    RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform.parent as RectTransform, eventData.position, eventData.pressEventCamera, out originalLocalPointerPosition);

    //}

    //public void OnPointerUp(PointerEventData eventData)
    //{
    //    float shortestDistance = float.MaxValue;
    //    Vector2 candidatePosition = Vector2.zero;

    //    foreach (KeyValuePair<int, Vector2> kvp in tabMenu.inventoryMatrix)
    //    {
    //        float distance = Vector2.Distance(rectTransform.anchoredPosition, kvp.Value);
    //        if (distance < shortestDistance)
    //        {
    //            shortestDistance = distance;
    //            candidatePosition = kvp.Value;
    //        }
    //    }

    //    rectTransform.anchoredPosition = candidatePosition;
    //    //onHover.toolTipCanOpen = true;
    //}

    //public void OnDrag(PointerEventData eventData)
    //{
    //    if (RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform.parent as RectTransform, eventData.position, eventData.pressEventCamera, out Vector2 localPointerPosition))
    //    {
    //        Vector3 offsetToOriginal = localPointerPosition - originalLocalPointerPosition;
    //        rectTransform.anchoredPosition = originalPanelLocalPosition + offsetToOriginal;
    //    }        
    //    //onHover.toolTipCanOpen = false;
    //}
}
