using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Console : MonoBehaviour
{
    public TMP_InputField inputField;
    private GameObject inputParent;
    private PlayerUnit player;
    [SerializeField] private InventoryManager inventoryManager;
    private EnemySpawner enemySpawner;
    private string lastCommand;
    [SerializeField] private GameObject itemDrop;

void Start()
    {
        player = FindObjectOfType<PlayerUnit>();
        inputParent = inputField.transform.parent.gameObject;
        inputParent.SetActive(false);
        inputField.onEndEdit.AddListener(OnSubmit);
        enemySpawner = FindObjectOfType<EnemySpawner>();
    }

    void Update() {
        if (inputParent.activeSelf) {
            if (Input.GetKeyDown(KeyCode.Escape)) {
                    inputParent.SetActive(false);
                    GameState.Instance.MenusOpen--;
                }
            else if (Input.GetKeyDown(KeyCode.UpArrow)) {
                inputField.text = lastCommand;
            }
        }
        else if (Input.GetKeyDown(KeyCode.Slash)) {
                GameState.Instance.MenusOpen++;
                inputParent.SetActive(true);
                inputField.ActivateInputField();
        }
    }

    void OnSubmit(string command)
    {
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            if (HandleCommand(command))
            {
                lastCommand = command;
            }
            inputField.text = ""; // Clear the input field after submitting
            inputParent.SetActive(false);
            GameState.Instance.MenusOpen--;
        }
    }

    private bool HandleCommand(string commandArg) {
        string command = commandArg.Trim();
        if (command.StartsWith("give "))
        {
            string itemName = command.Substring(5); // Extract item name
            ItemData item = inventoryManager.GetItemFromCommandName(itemName);
            inventoryManager.Equip(item);

            if (item == null) {
                Debug.LogWarning("'" + itemName + "'"  + " was not a recognized item");
            }
            else {
                return true;
            }
        }
        else if (command.StartsWith("set "))
        {
            string remainder = command.Substring(4);
            int lastSpaceIndex = remainder.LastIndexOf(' ');
            if (lastSpaceIndex == -1)
            {
                return false;
            }
            string itemName = remainder.Substring(0, lastSpaceIndex);
            string indexStr = remainder.Substring(lastSpaceIndex + 1);

            // Convert the index string to an integer
            if (!int.TryParse(indexStr, out int index) || index < 1)
            {
                Debug.LogError("Invalid index provided.");
                return false;
            }

            // Adjust for 0-based index (assuming user input is 1-based)
            index -= 1;
            ItemData item = inventoryManager.GetItemFromCommandName(itemName);
            inventoryManager.SetItemToHotbar(item, index);
            //Debug.Log($"Item '{itemName}' set to hotbar position {index + 1}");
            if (item != null) return true;
        }
        else if (command.StartsWith("setres ")) {
            string arg = RemainderAfterFirstSpace(command);
            switch (arg)
            {
                case "1":
                    Screen.SetResolution(1024, 576, false);
                    return true;
                case "2":
                    Screen.SetResolution(1280, 720, false);
                    return true;
                case "3":
                    Screen.SetResolution(1600, 900, false);
                    return true;
                case "4":
                    Screen.SetResolution(1920, 1080, true);
                    return true;
                default:
                    return false;
            }
        }
        else if (command.StartsWith("undamageable"))
        {
            player.ApplySTFX(Unit.STFX.Undamageable, 7f);
            return true;
        }
        else if (command.StartsWith("stasis"))
        {
            player.ApplySTFX(Unit.STFX.Stasis);
            return true;
        }
        //else if (command.StartsWith("attackradius ")) {
        //    string value = command.Substring(13);
        //    if (float.TryParse(value, out float result)) {
        //        playerStats.attackRange += result;
        //    }
        //    else {
        //        Debug.LogError("unknown argument " + value + " in '" + command + "'");
        //    }
        //}
        //else if (command.StartsWith("attackspeed "))
        //{
        //    string value = command.Substring(12);
        //    if (float.TryParse(value, out float result))
        //    {
        //        playerStats.attackSpeed += result;
        //        GameState.Instance.PlayerStats.SetStatModifier(UnitStats.Stat.AttackSpeed)
        //    }
        //    else
        //    {
        //        Debug.LogError("unknown argument " + value + " in '" + command + "'");
        //    }
        //}
        else if (command.StartsWith("god "))
        {
            string arg = RemainderAfterFirstSpace(command);
            if (arg == "t")
            {
                player.ApplySTFX(Unit.STFX.Undamageable);
                player.ApplySTFX(false, Unit.Stat.Damage, 999f, 0, true);
                return true;
            }
            else if (arg == "f")
            {
                player.GetComponent<Undamageable>()?.Clear();
                return true;
            }
            else { return false; }
        }
        else if (command.StartsWith("takedmg "))
        {
            string value = command.Substring(8);
            if (float.TryParse(value, out float result))
            {
                player.TakeDamage(new Attack(Attack.Type.Console, player, result));
                return true;
            }
            else
            {
                Debug.LogError("unknown argument " + value + " in '" + command + "'");
            }
        }
        if (command.StartsWith("s ") || command.StartsWith("spawn "))
        {
            string[] parts = command.Split(' '); // Split the command into parts
            string entityType = parts.Length > 1 ? parts[1] : string.Empty; // Entity type to spawn
            int count = 1; // Default count

            // Check if there's a third part and it's an integer
            if (parts.Length > 2 && int.TryParse(parts[2], out int parsedCount))
            {
                count = parsedCount; // Set count to the parsed number if available and valid
            }

            bool anyValid = false; // To track if any spawn commands are valid
            for (int i = 0; i < count; i++)
            {
                bool result = enemySpawner.SpawnFromConsole(entityType); // Call spawn method
                if (!result)
                {
                    switch (entityType)
                    {
                        case "itemDrop":
                            Vector3 vector = new Vector3(-2, 0, 0);
                            Instantiate(itemDrop, GameState.Instance.PlayerTransform.position + vector, Quaternion.identity);
                            result = true; // Assume Instantiate is successful
                            break;
                        default:
                            Debug.LogError("Unknown argument " + entityType + " in '" + command + "'");
                            result = false;
                            break;
                    }
                }
                anyValid = anyValid || result; // If any command is valid, mark as valid
            }
            return anyValid;
        }

        else if (command == "count menusOpen") {
            Debug.Log(GameState.Instance.MenusOpen.ToString());
            return true;
        }
        else if (command == "mute") {
            AudioListener.volume = 0;
            return true;
        }
        else if (command == "unmute") {
            AudioListener.volume = 1;
            return true;
        }
        else if (command == "die") {
            player.Kill();
            return true;
        }
        else if (command == "p") {
            if(GameState.Instance.MenusOpen >=  2) GameState.Instance.MenusOpen--;
            else GameState.Instance.MenusOpen++;
            return true;
        }
        else if (command == "kill all") {
            foreach (Transform child in enemySpawner.transform)
            {
                child.gameObject.GetComponent<EnemyUnit>()?.Kill();
            }
            return true;
        }
        else if (command == "win") {
            FindFirstObjectByType<GameOver>().GameEnd(true);
            return true;
        }
        else if (command == "") {
            Debug.LogWarning("command cannot be empty.");
            return false;
        }
        else
        {
            Debug.LogWarning("Unknown command: " + command);
            return false;
        }
    }
    private string RemainderAfterFirstSpace(string input) {
        int spaceIndex = input.IndexOf(' ');
        if (spaceIndex >= 0)
        {
            return input.Substring(spaceIndex + 1);
        }
        return string.Empty;
    }

    private void MultipleEquip(ItemData item, int times) {
        for (int i = 0; i < times; i++) {
            inventoryManager.Equip(item);
        }
    }

}