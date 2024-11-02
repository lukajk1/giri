using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HB_Tickdown : MonoBehaviour
{
    //fields to pass to 
    [SerializeField] private Slider healthSlider;
    [SerializeField] private Slider whiteHealthSlider;
    [SerializeField] private Slider attackCooldownBar;

    [SerializeField] private Slider shieldSlider;
    [SerializeField] private Slider whiteShieldSlider;

    private Unit unit;
    private float currentHealth;
    //private bool healthTickDownIsRunning;
    private const float TICKDOWN_SPEED = 0.3f;

    private UnitController unitController;

    private Coroutine healthTickdown = null;
    private Coroutine shieldTickdown = null;
    public void Initialize(Unit unit)
    {
        this.unit = unit;

        healthSlider.value = 1;
        whiteHealthSlider.value = 1;
        currentHealth = unit.CurrentHealth;

        shieldSlider.value = unit.Shield / unit.CurrentHealth;
        whiteShieldSlider.value = 0;

        //ShieldUpdated();

        if (unit is EnemyUnit)
        {
            unitController = unit.gameObject.GetComponent<EnemyController>();
        }
        if (unit is PlayerUnit)
        {
            PlayerUnit player = (PlayerUnit)unit;
            unitController = player.AttackOriginMarker.GetComponent<PlayerController>();
        }

        unit.HealthUpdated += HealthUpdated;
        unit.ShieldUpdated += ShieldUpdated;

        shieldTickdown = null;
    }
    void Update()
    {
        attackCooldownBar.value = unitController.GetCooldownStatus();
        //Debug.Log($"{unit.gameObject.name} shield tickdown running; {shieldTickdown == null}");
        //Debug.Log($"{healthTickdown == null} health tickdown is running");
    }

    private void HealthUpdated(bool isLowered)
    {
        //Debug.Log("called health updated");
        healthSlider.value = unit.CurrentHealth / unit.MaxHealth;
        currentHealth = unit.CurrentHealth;

        if (healthTickdown == null && isLowered)
        {
            healthTickdown = StartCoroutine(TickDown(healthSlider, whiteHealthSlider));
        }
    }

    private void ShieldUpdated(bool isLowered)
    {
        shieldSlider.value = unit.Shield / unit.MaxHealth;
        //Debug.Log($"{unit.name} {unit.Shield}");

        if (shieldTickdown == null && isLowered)
        {
            shieldTickdown = StartCoroutine(TickDown(shieldSlider, whiteShieldSlider));
        }
    }

    private IEnumerator TickDown(Slider mainSlider, Slider whiteSlider)
    {
        float startingValue = whiteSlider.value;
        float timeElapsed = 0f;
        GameState state = GameState.Instance;

        while (timeElapsed < TICKDOWN_SPEED)
        {
            while (state.MenusOpen > 0) yield return null;
            yield return null;
            timeElapsed += Time.deltaTime;
            whiteSlider.value = Mathf.Lerp(startingValue, mainSlider.value, timeElapsed / TICKDOWN_SPEED);
        }

        whiteSlider.value = mainSlider.value; // make sure they end up being the same
        
        if (mainSlider == healthSlider) healthTickdown = null;
        else if (mainSlider == shieldSlider) shieldTickdown = null;
    }

}
