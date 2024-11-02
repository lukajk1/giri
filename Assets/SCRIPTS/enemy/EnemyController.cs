using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : UnitController
{
    private GameObject enemyList;
    private EnemyUnit unit;
    private SpriteRenderer model;
    private Cooldown cooldown;
    public IEnemyAttack attack;

    private float proximityThreshold = 2.5f; // Distance to check for nearby enemies
    private float randomMoveRadius = 2f; // Radius to pick a random point to move to
    private Vector2? randomDestination = null; // Nullable Vector2 to store random destination

    private WalkingMotion walkingMotion;
    private AttackScriptable attackData;
    public enum State
    {
        Moving, WaitingForCooldown, Attacking
    }
    private State _state;
    private State state
    {
        get { return _state; }
        set
        {
            _state = value;
            ConfigureOnStateChange();
        }
    }
    private bool isAttacking = false;
    public void Initialize(SpriteRenderer model, AttackScriptable attackData, EnemyData data)
    {
        enemyList = GameObject.Find("Enemy Manager");
        unit = gameObject.GetComponent<EnemyUnit>();
        walkingMotion = model.gameObject.GetComponent<WalkingMotion>();

        this.model = model;
        this.attackData = attackData;

        //Debug.Log(enemy);
        //Debug.Log(walkingMotion);

        state = State.Moving;
    }
    void Update()
    {
        if (isAttacking || unit.isDying)
        {
            return; // just sit here mutely basically
        }
        else if (FindDistanceFromPlayer() <= unit.AttackRange)
        {
            if (cooldown == null)
            {
                state = State.Attacking;
                Attack();
            }
            else
            {
                state = State.WaitingForCooldown;
                // do nothing and do not walk if you are groudn unit
            }
        }
        else if (state != State.Attacking) 
        {

            state = State.Moving;
            MoveTowardsPlayer();
        }

        LookAtPlayer();
    }

    private void ConfigureOnStateChange()
    {
        switch (state)
        {
            case State.Moving:
                AllowMovement(true);
                break;
            case State.Attacking:
            case State.WaitingForCooldown:
                AllowMovement(false);
                break;
        }
    }
    public void AllowMovement(bool value)
    {
        if (!unit.FlyingUnit)
        {
            walkingMotion.WalkingEnabled = value;
        }
    }
    private void Attack()
    {
        cooldown = gameObject.AddComponent<Cooldown>();
        cooldown.Duration = attackData.Cooldown;
        cooldown.Initiate();
        attack.Attack(attackData);
        StartCoroutine(Attacking());
    }

    private IEnumerator Attacking()
    {
        isAttacking = true;
        float timeElapsed = 0f;
        GameState gs = GameState.Instance;
        while (timeElapsed < (attackData.WindupLength + EnemyAttack.HITBOX_ACTIVE_LENGTH) + 0.4f) // the additional .2 is just because enemies keep moving before they're supposed to by a constant amount, I don't know why, it'll just look like this is part of intended behavior anyhow ig
        {
            if (gs.MenusOpen > 0) yield return null;
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        state = State.WaitingForCooldown;
        isAttacking = false;
    }

    public override float GetCooldownStatus()
    {
        if (cooldown == null) return 1;
        else
        {
            return cooldown.Progress;
        }
    }
    void MoveTowardsPlayer()
    {
        if (GameState.Instance.MenusOpen != 0 || unit.UnitStatus == Unit.Status.Dazed || unit.CheckForSTFX<Root>()) return;

        Vector2 moveTarget;

        if (IsEnemyNearby() || randomDestination.HasValue)
        {
            if (!randomDestination.HasValue)
            {
                // Pick a random nearby point in the general direction of the player
                randomDestination = GetRandomPointTowardsPlayer();
            }

            moveTarget = randomDestination.Value;

            // Check if we've reached the random destination
            if (Vector2.Distance(transform.position, moveTarget) < 0.1f)
            {
                randomDestination = null; // Clear the random destination
            }
        }
        else
        {
            moveTarget = GameState.Instance.PlayerTransform.position;
        }

        Vector2 tentativePosition = Vector2.MoveTowards(transform.position, moveTarget, unit.MoveSpeed * Time.deltaTime);
        transform.position = tentativePosition;
    }

    private void LookAtPlayer()
    {
        if (GameState.Instance.PlayerTransform.position.x > transform.position.x && !model.flipX)
        {
            model.flipX = true;
        }
        else if (GameState.Instance.PlayerTransform.position.x < transform.position.x && model.flipX)
        {
            model.flipX = false;
        }
    }

    Vector2 GetRandomPointTowardsPlayer()
    {
        Vector2 directionToPlayer = (GameState.Instance.PlayerTransform.position - transform.position).normalized;

        // Generate a random angle within a 90-degree arc towards the player
        float randomAngle = Random.Range(-45f, 45f);
        Vector2 randomDirection = Quaternion.Euler(0, 0, randomAngle) * directionToPlayer;

        // Generate a random distance within the move radius
        float randomDistance = Random.Range(0.5f * randomMoveRadius, randomMoveRadius);

        return (Vector2)transform.position + randomDirection * randomDistance;
    }

    bool IsEnemyNearby()
    {
        foreach (Transform enemyTransform in enemyList.transform)
        {
            if (enemyTransform != transform && Vector2.Distance(transform.position, enemyTransform.position) < proximityThreshold)
            {
                return true;
            }
        }
        return false;
    }

    protected float FindDistanceFromPlayer()
    {
        return Vector2.Distance(transform.position, GameState.Instance.PlayerTransform.position);
    }
}