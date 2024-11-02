using System.Collections.Generic;
using UnityEngine;

public class KeybindMap : MonoBehaviour
{
    public Dictionary<string, KeyCode> KeyMap;
    public Dictionary<string, int> MouseMap;

    void Awake()
    {
        KeyMap = new Dictionary<string, KeyCode>
        {
            { "center camera", KeyCode.Space },
            { "slot 1", KeyCode.Q },
            { "slot 2", KeyCode.W },
            { "slot 3", KeyCode.E },
            { "slot 4", KeyCode.R },
            { "slot 5", KeyCode.D },
            { "slot 6", KeyCode.F },
            { "attack move", KeyCode.S },
            { "show attack radius", KeyCode.A },
            { "show tab menu", KeyCode.Tab }
        };

        MouseMap = new Dictionary<string, int>
        {
            { "click", 0 }, // Left mouse button
            { "move click", 1 },       // Right mouse button
        };
    }
}