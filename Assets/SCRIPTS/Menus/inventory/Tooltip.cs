using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Text.RegularExpressions;

public class Tooltip : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI title;
    [SerializeField] private TextMeshProUGUI description;
    [SerializeField] private TextMeshProUGUI flavorText;
    private string defaultDescription;

    private Sprite imgSprite;
    void Start() {
        //gameObject.SetActive(false);
        defaultDescription = description.text;
        title.text = "";
    }

    public void Exit()
    {
        description.text = defaultDescription;
        title.text = "";
        flavorText.text = "";
        foreach (Transform child in statsParent.transform)
        {
            Destroy(child.gameObject);
        }
    }

    public void Open(ItemData itemData) {
        if (itemData != null)
        {
            ColorAndSetTitle(itemData);
            description.text = HighlightKeywords(itemData.Description, itemData.Active != "", itemData.Cooldown);
            flavorText.text = itemData.FlavorText;
            SetStats(itemData);
        }
    }
    [SerializeField] private GameObject statsParent;
    [SerializeField] private GameObject statLabel;
    private void SetStats(ItemData itemData)
    {
        GameObject statInstance = null;
        foreach (KeyValuePair<string, float> kvp in itemData.statNameDictionary)
        {
            if (kvp.Value != 0)
            {
                statInstance = Instantiate(statLabel, statsParent.transform);
                statInstance.GetComponent<TextMeshProUGUI>().text = kvp.Key;
                statInstance.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = kvp.Value.ToString();
            }
        }
    }


    [SerializeField] private Color activeColor;
    [SerializeField] private Color passiveColor;
    [SerializeField] private Color uniqueColor;
    string HighlightKeywords(string inputText, bool hasActive, float cooldown)
    {
        string htmlActiveColor = ColorUtility.ToHtmlStringRGB(activeColor);
        string htmlPassiveColor = ColorUtility.ToHtmlStringRGB(passiveColor);
        string htmlUniqueColor = ColorUtility.ToHtmlStringRGB(uniqueColor);

        // Coloring "Unique Passive"
        inputText = Regex.Replace(inputText, @"(Unique Passive[^:]*:)", $"<color=#{htmlUniqueColor}>$1</color>");

        // Coloring "Unique Active"
        inputText = Regex.Replace(inputText, @"(Unique Active[^:]*:)", $"<color=#{htmlUniqueColor}>$1</color>");

        // Coloring "Passive" not preceded directly by "Unique"
        inputText = Regex.Replace(inputText, @"(?<!Unique )(Passive[^:]*:)", $"<color=#{htmlPassiveColor}>$1</color>");

        // Coloring "Active" not preceded directly by "Unique"
        inputText = Regex.Replace(inputText, @"(?<!Unique )(Active[^:]*:)", $"<color=#{htmlActiveColor}>$1</color>");

        if (hasActive)
        {
            inputText += $" {cooldown}s cooldown.";
        }

        return inputText;
    }

    [SerializeField] private Color rarity1Color; // Default to gray
    [SerializeField] private Color rarity2Color; // Custom pink

    private void ColorAndSetTitle(ItemData itemData)
    {
        if (title == null || itemData == null)
        {
            Debug.LogError("Title or ItemData is not assigned or is null.");
            return;
        }

        switch (itemData.Tier)
        {
            case 1:
                title.color = rarity1Color;
                break;
            case 2:
                title.color = rarity2Color;
                break;
            default:
                title.color = Color.white; // Default to white if rarity is not recognized
                break;
        }
        title.text = itemData.VanityItemName;
    }
}
