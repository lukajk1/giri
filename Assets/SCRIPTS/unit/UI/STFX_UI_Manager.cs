using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class STFX_UI_Manager : MonoBehaviour
{

    [SerializeField] private GameObject statusIconList;
    [SerializeField] private GameObject statusIcon;

    [Header("State Icons (over the unit's head)")]
    [SerializeField] private GameObject hardCCSymbol;
    [SerializeField] private Sprite dazeSymbol;
    [SerializeField] private Sprite rootSymbol;

    [Header("Normal Status Icons (above the healthbar)")]
    [SerializeField] private Sprite spearedSymbol;
    [SerializeField] private Sprite bleedSymbol;

    private Unit unit;
    //private int statusCount = 0;

    public void Initialize(Unit unit)
    {
        this.unit = unit;
        unit.StatusUpdated += StatusUpdated;
    }

    private void StatusUpdated()
    {
        foreach (Transform child in statusIconList.transform) // clear list first
        {
            Destroy(child.gameObject);
        }

        ISTFX[] effects = unit.gameObject.GetComponents<ISTFX>();
        //Debug.Log(effects.Length);
        foreach (ISTFX effect in effects)
        {
            if (effect is not StateEffect)
            {
                GameObject icon = Instantiate(statusIcon, statusIconList.transform);
                Sprite sprite = null;

                if (effect is Bleed) {
                    sprite = bleedSymbol;
                }
                else if (effect is Speared) sprite = spearedSymbol;


                if (sprite != null) icon.GetComponent<Image>().sprite = sprite;
                icon.SetActive(true);
            }
        }

        switch (unit.UnitStatus)
        {
            case Unit.Status.Dazed:
                SetCCSymbol(dazeSymbol);
                break;
            case Unit.Status.Rooted:
                SetCCSymbol(rootSymbol);
                break;
            case Unit.Status.Default:
                hardCCSymbol.SetActive(false);
                break;
            default:
                break;
        }
    }

    private void SetCCSymbol(Sprite sprite)
    {
        hardCCSymbol.GetComponent<Image>().sprite = sprite;
        hardCCSymbol.SetActive(true);
    }

}
