using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Backgrounds : MonoBehaviour
{
    [SerializeField] private Sprite outskirts;
    [SerializeField] private Sprite interior;
    [SerializeField] private bool isOutskirts;
    private void Start()
    {
        GetComponent<SpriteRenderer>().sprite = isOutskirts ? interior : outskirts;
    }
}
