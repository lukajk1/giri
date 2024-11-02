using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HB_Setup : MonoBehaviour {
    //[SerializeField] private RectTransform tick;
    //[SerializeField] private GameObject healthbar;
    public void Initialize(Unit unit, UnitData data) {
        Canvas healthbarCanvas;
        if (data is PlayerData)
        {
            PlayerData player = (PlayerData)data; // cast to PlayerData
            healthbarCanvas = player.HealthbarCanvas;
        }
        else // enemydata
        {
            healthbarCanvas = GameAssets.Instance.HealthBarCanvas;
        }
        GameObject hbCanvasInstance = Instantiate(healthbarCanvas.gameObject, gameObject.transform.Find("healthbar position"));

        RectTransform tick = hbCanvasInstance.gameObject.transform.Find("healthbar").Find("healthbar tick").gameObject.GetComponent<RectTransform>();
        GameObject healthbar = hbCanvasInstance.gameObject.transform.Find("healthbar").gameObject;

        HB_Ticks hb_Ticks = gameObject.AddComponent<HB_Ticks>();
        hb_Ticks.Initialize(unit.MaxHealth, tick, healthbar);

        STFX_UI_Manager cc = hbCanvasInstance.GetComponent<STFX_UI_Manager>();
        cc.Initialize(unit);

        hbCanvasInstance.GetComponent<HB_Tickdown>().Initialize(unit);
    }

}
