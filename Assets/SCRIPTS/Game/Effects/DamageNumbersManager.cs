using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageNumbersManager : MonoBehaviour
{
    [SerializeField] private Canvas damageCanvas;
    public static DamageNumbersManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogError("more than one DamageNumbersManager instance in scene");
        }
    }

    public bool CreateMessage(float damage, bool isCrit, Vector3 pos) // boolean outs currently not used
    {
        Canvas dmg = Instantiate(damageCanvas, pos + new Vector3(0, 2.0f, 0), Quaternion.identity);
        dmg.GetComponent<DamageNumbers>().Initialize(damage, isCrit);
        return true;
    }
    public bool CreateMessage(string message, Vector3 pos)
    {
        Canvas dmg = Instantiate(damageCanvas, pos + new Vector3(0f, 1.0f, 0), Quaternion.identity);
        dmg.GetComponent<DamageNumbers>().Initialize(message);
        return true;
    }
}
