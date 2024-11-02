using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CastingAttackInstance : EnemyAttack 
{
    private EnemyData enemyData;
    private AttackScriptable data;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private SpriteRenderer sprite;
    private Transform target;

    public void Initialize(AttackScriptable data, EnemyData enemyData) {
        this.enemyData = enemyData;
        this.data = data;
        GetComponent<CastHitbox>().Initialize(data);

        DetermineAttackColor(sprite, data);

        target = GameState.Instance.Player.Model.transform;
        StartCoroutine(CastTrajectory());
    }

    private IEnumerator CastTrajectory() {
        float timeElapsed = 0f;
        Vector3 direction = (target.position - transform.position).normalized;
        GameState gs = GameState.Instance;
        transform.position += new Vector3(0, 1f, 0); // small vertical offset so attack comes from closer to the midsection of the unit rather than their feet

        while (timeElapsed < data.WindupLength) {
            while (gs.MenusOpen > 0) yield return null;

            rb.MovePosition(rb.position + (Vector2)direction * 0.01f * 7.5f); // projectile speed is last figure
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        
        Destroy(gameObject);
    }
}
