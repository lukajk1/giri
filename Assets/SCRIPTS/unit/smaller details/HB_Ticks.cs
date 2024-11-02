using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HB_Ticks : MonoBehaviour {
    private RectTransform tick;
    private GameObject healthbar;
    void Start() {
    }
    public void Initialize(float health, RectTransform tick, GameObject healthbar) {
        this.healthbar = healthbar;
        this.tick = tick;

        float interval = 85f / health * 100; // 85 is the width of the parent bounding box/healthbar
        float totalTicks = health / 100;
        int intervalsInteger = (int)Mathf.Floor(totalTicks);

        bool overOneThousandHP = health > 1000f;

        for (int i = 1; i <= totalTicks; i++) {
            GameObject tickInstance = Instantiate(tick.gameObject, healthbar.transform);
            if (i % 10 == 0) {
                tickInstance.GetComponent<RectTransform>().sizeDelta = new Vector2(2.1f, 8);
                tickInstance.GetComponent<RectTransform>().anchoredPosition = new Vector2(interval * i, 0);
            }
            else {
                if (health < 5000) {
                    tickInstance.GetComponent<RectTransform>().sizeDelta = new Vector2(1, 4);
                    tickInstance.GetComponent<RectTransform>().anchoredPosition = new Vector2(interval * i, 2);
                }
            }
            tickInstance.SetActive(true);
        }
    }

    public void ClearTicks() {
        //Transform healthbar = healthbarCanvasInstance.gameObject.transform.Find("healthbar");

        int childCount = healthbar.transform.childCount;

        if (childCount < 2)
        {
            return;
        }

        for (int i = 2; i < childCount; i++)
        {
            Destroy(healthbar.transform.GetChild(i).gameObject);
        }
    
    }


}
