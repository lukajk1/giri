using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class HUDSlot : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI remainingCooldown;
    [SerializeField] private TextMeshProUGUI keybind;
    [SerializeField] private Image cooldownWipe;
    [SerializeField] private Image image;
    [SerializeField] private Material glowMaterial;
    //public KeyCode ButtonActive {  get; set; }

    private Cooldown cooldown;
    private ItemActivatable active;
    private ItemData item;
    public void SetSlot(ItemData argItem)
    {
        if (argItem == null) return;
        image.enabled = true;
        item = argItem;
        image.sprite = item.Icon;

        GameObject g = gameObject;
        switch (item.Active)
        {
            case "IAHalveHealth":
                active = g.AddComponent<IAHalveHealth>();
                break;
            case "IAEmpower":
                active = g.AddComponent<IAEmpower>();
                break;
            case "IABlink":
                active = g.AddComponent<IABlink>();
                break;
            case "IAPurge":
                active = g.AddComponent<IAPurge>();
                break;
            case "IAPurify":
                active = g.AddComponent<IAPurify>();
                break;
            case "IATargetedDash":
                active = g.AddComponent<IATargetedDash>();
                break;
            case "IASwapPositions":
                active = g.AddComponent<IASwapPositions>();
                break;
            case "IAShield":
                active = g.AddComponent<IAShield>();
                break;
            case "IAStasis":
                active = g.AddComponent<IAStasis>();
                break;
            case "IAPercentageShield":
                active = g.AddComponent<IAPercentageShield>();
                break;
            case "IANileDagger":
                active = g.AddComponent<IANileDagger>();
                break;
            case "IANileDash":
                active = g.AddComponent<IANileDash>();
                break;
            default:
                Debug.LogWarning($"did not find matching component for {item.VanityItemName} {item.Active}");
                return;
        }
        active.CooldownWipe = cooldownWipe;
        cooldownWipe.fillAmount = 0;
        //Debug.Log(item.VanityItemName);
    }

    public void Activate()
    {
        if (item == null) return;
        else if (item.Active == "")
        {
            Debug.Log("item does not have active!"); // although I guess all items by definition placable in hotbar should have an active
            return;
        }

        if (cooldown == null && active.Activate())
        {
            GameState.Instance.Player.TotalActivesUsed++;
            cooldown = gameObject.AddComponent<Cooldown>();
            cooldown.Duration = item.Cooldown * GameState.Instance.Player.CooldownLength;
            cooldown.Initiate();

        }
        else if (cooldown != null)
        {
            GameAssets.Instance.Alert(GameAlert.Reason.CooldownNotUp);
            GameState.Instance.Audio.PlaySound(ADFM.Sfx.CDNotUp);
        }
    }

    void Update()
    {
        if (cooldown != null)
        {
            cooldownWipe.fillAmount = (cooldown.Duration - cooldown.TimeElapsed) / cooldown.Duration;
            if (cooldown.Duration - cooldown.TimeElapsed > 10f)
            {
                remainingCooldown.text = (cooldown.Duration - cooldown.TimeElapsed).ToString("0") + "s";
            }
            else
            {
                remainingCooldown.text = (cooldown.Duration - cooldown.TimeElapsed).ToString("F1") + "s"; // Format to one decimal place
            }
        }
        if (cooldown == null && remainingCooldown.text != "")
        {
            remainingCooldown.text = string.Empty;
            GameState.Instance.Audio.PlaySound(ADFM.Sfx.CDUp);
            StartCoroutine(GlowOffCD());
        }
    }

    public void SetKeybindName(string keybindLabel)
    {
        keybind.text = keybindLabel;
    }

    private IEnumerator GlowOffCD()
    {
        Material originalMat = image.material;
        Material instanceGlowMaterial = new Material(glowMaterial);

        //image.material = new Material(glowMaterial);
        image.material = instanceGlowMaterial;

        float timeElapsed = 0;
        float duration = 0.25f;
        float halfDuration = duration / 2;
        GameState state = GameState.Instance;


        float glowValueMax = 17f;

        int glowStrengthProperty = Shader.PropertyToID("_Strength");

        while (timeElapsed < halfDuration)
        {
            while (state.MenusOpen > 0) yield return null;
            instanceGlowMaterial.SetFloat(glowStrengthProperty, Mathf.Lerp(0, glowValueMax, timeElapsed / halfDuration));
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        timeElapsed = 0f;
        while (timeElapsed < halfDuration)
        {
            while (state.MenusOpen > 0) yield return null;
            instanceGlowMaterial.SetFloat(glowStrengthProperty, Mathf.Lerp(glowValueMax, 0, timeElapsed / halfDuration));
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        image.material = originalMat;
    }
}
