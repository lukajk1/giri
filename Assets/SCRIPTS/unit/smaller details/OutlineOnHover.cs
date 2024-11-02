using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutlineOnHover : MonoBehaviour
{
    [SerializeField] private Material defaultMaterial;
    [SerializeField] private Material outlineMaterial;
    private SpriteRenderer spriteRenderer;
    public bool outlineFunctionEnabled = true;
    void Start()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }
    void Update() {
        if (GameState.Instance.MenusOpen != 0) {
            gameObject.GetComponent<CapsuleCollider2D>().enabled = false;
        }
        else {
            gameObject.GetComponent<CapsuleCollider2D>().enabled = true;
        }
        if (!outlineFunctionEnabled && spriteRenderer.material == outlineMaterial)
        {
            spriteRenderer.material = defaultMaterial;
        }
    }

    void OnMouseEnter(){
        if (outlineFunctionEnabled)
        {
            spriteRenderer.material = outlineMaterial;
        }
    }

    void OnMouseExit() {
        if (outlineFunctionEnabled)
        {
            spriteRenderer.material = defaultMaterial;
        }
    }
}
