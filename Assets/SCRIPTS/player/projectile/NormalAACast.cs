using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class NormalAACast : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private float speed;
    public PlayerUnit stats;
    private bool arcDirection;
    private EnemyUnit target;
    private bool isCrit;
    void Update()
    {
        if (target == null) Destroy(gameObject);
    }
    public void ShootAt(EnemyUnit target, float damage, bool isCrit, bool arcDir) {
        this.target = target;
        this.isCrit = isCrit; 
        arcDirection = arcDir;
        StartCoroutine(MoveTowards(target.transform, speed, damage, isCrit));
    }
    IEnumerator MoveTowards(Transform target, float speed, float damage, bool isCrit)
    {
        Vector3 startPos = transform.position;
        Vector3 targetPos = target.position;

        float distance = Vector3.Distance(startPos, targetPos);
        float height = Random.Range(0.45f, 0.8f); // Randomly varying height for the parabolic motion
        height = arcDirection ? -height : height;

        float duration = distance / speed; // Time to reach the target

        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            while (GameState.Instance.MenusOpen > 0) yield return null; 

            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;

            // Interpolating position along the horizontal plane
            Vector3 currentPos = Vector3.Lerp(startPos, targetPos, t);

            // Adding parabolic height
            currentPos.y += height * Mathf.Sin(Mathf.PI * t);

            transform.position = currentPos;

            yield return null;
        }

        EnemyUnit enemy = target.gameObject.GetComponent<EnemyUnit>();
        if (enemy != null)
        {
            enemy.TakeDamage(new Attack(Attack.Type.AutoAttack, enemy, stats.Damage, isCrit));
            
        }

        //float lifeStolen = damage * stats.lifesteal;
        //stats.Heal(lifeStolen);
        //if (stats.currentHealth < stats.maxHealth && lifeStolen > 0)
        //{
        //        GameState.Instance.audioManager.PlaySound("playerHealing");
        //}

        Destroy(gameObject);
    }
}
