using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAssets : MonoBehaviour
{
    public static GameAssets Instance { get; private set; }
    public Canvas DamageCanvas;
    public Canvas HealthBarCanvas;

    [SerializeField] private GameObject gameAlert;
    public GameObject GameAlert => gameAlert;

    [Header("Particles")]
    [SerializeField] public GameObject DeathParticles;
    [SerializeField] public GameObject HitParticles;
    [SerializeField] public GameObject HUD;

    [Header("Materials")]
    public Material Stasis;
    public Material Rooted;
    public Material Dazed;
    public Material Cleanse;
    public Material Purge;
    public Material Damaged;

    //[SerializeField] private Canvas healthCanvas;
    //public Canvas HealthCanvas => healthCanvas; // I don't feel like switching over the references yet... 
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private Cooldown alertCd;
    public void Alert(GameAlert.Reason reason)
    {
        if (alertCd == null)
        {
            GameObject _alert = Instantiate(gameAlert, HUD.transform);
            _alert.GetComponent<GameAlert>().Initiate(reason);
            alertCd = gameObject.AddComponent<Cooldown>();
            alertCd.Duration = 0.5f;
            alertCd.Initiate();
        }
    }
}
