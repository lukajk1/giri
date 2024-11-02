using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FieldWindup : EnemyAttack
{
    private AttackScriptable data;
    [SerializeField] private Collider2D myCollider;
    [SerializeField] private SpriteRenderer sprite;

    public void Initialize(AttackScriptable data) {
        if (myCollider == null || sprite == null) {
            Debug.LogError("assignment missing in the inspector");
        }
        //if (sprite.enabled)
        //{
        //    Debug.LogError("sprite is supposed to start disabled");
        //}

        this.data = data;
        //Debug.Log(attackWindupLength);
        myCollider.enabled = false;

        DetermineAttackColor(sprite, data);
        sprite.enabled = true;
        StartCoroutine(Windup());
    }
    private IEnumerator Windup()
    {
        GameState gs = GameState.Instance;
        float timeElapsed = 0f;
        Color initialColor = sprite.color;

        while (timeElapsed < data.WindupLength)
        {
            while (gs.MenusOpen > 0) yield return null;
            yield return new WaitForSeconds(0.01f);
            timeElapsed += 0.01f;
            float alpha = Mathf.Lerp(0, 1, timeElapsed / data.WindupLength);
            sprite.color = new Color(initialColor.r, initialColor.g, initialColor.b, alpha);
        }

        sprite.color = new Color(1, 1, 1, 0.85f); // slightly transparent white
        timeElapsed = 0;
        myCollider.enabled = true;

        while (timeElapsed < HITBOX_ACTIVE_LENGTH) {
            while (gs.MenusOpen > 0) yield return null;
            yield return new WaitForSeconds(0.01f);
            timeElapsed += 0.01f;

        }
        
        Destroy(gameObject);

    }
}
