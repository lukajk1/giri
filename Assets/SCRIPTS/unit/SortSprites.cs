using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SortSprites : MonoBehaviour
{
    
    private Unit[] unitsList;
    public void RebuildSpriteList()
    {
        unitsList = FindObjectsOfType<Unit>();
    }

    void Update()
    {
        if (unitsList != null) OrderSprites();
    }

    private void OrderSprites()
    {
        //foreach (Unit unit in unitsList)
        //{
        //    if (unit != null)
        //    {
        //        SpriteRenderer spriteRenderer;
        //        if (unit.FlyingUnit)
        //        {
        //            spriteRenderer = unit.transform.Find("model").GetComponent<SpriteRenderer>();
        //        }
        //        else
        //        {
        //            spriteRenderer = unit.transform.Find("pivot").Find("model").GetComponent<SpriteRenderer>();
        //        }
                
        //        if (spriteRenderer != null)
        //        {
        //            int sortingOrder = Mathf.FloorToInt(unit.transform.position.y * -100);
        //            spriteRenderer.sortingOrder = sortingOrder;
        //        }
        //    }
        //}
    }
}
