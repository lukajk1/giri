using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceDirection : MonoBehaviour
{
    // Start is called before the first frame update
    private SpriteRenderer sprite;
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {


    }

    public void DetermineFacingDirection(Vector3 targetPosition) { // sprite faces to the left by default -> flipX == true is facing right

        if (targetPosition.x > transform.position.x && !sprite.flipX) {
            sprite.flipX = true;
        }
        else if (targetPosition.x < transform.position.x && sprite.flipX) {
            sprite.flipX = false;
        }
    }
}
