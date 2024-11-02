using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : UnitController
{
    [SerializeField] private GameObject playerModel;
    [SerializeField] private GameObject enemyListParent;
    [SerializeField] private PlayerUnit player;

    private Vector2 targetPosition;
    private Coroutine attackCoroutine;
    private bool hasAutoed;

    [SerializeField] private GameObject moveClick;
    [SerializeField] private GameObject attackClick;
    private Transform attackOrigin;

    private bool attackIsUp;
    private Cooldown attackCooldown;
    private bool canInputMovementCommands = true;
    private bool attackBuffered = false;
    private GameObject attackBufferTarget;

    [SerializeField] private AutoAttack autoAttack;
    void Start() {
        attackOrigin = transform;

        targetPosition = transform.parent.position;

        //switch (player.Data.unitCommandName) {
        //    case "nile":
        //        autoAttack = gameObject.GetComponent<SpearAutoAttack>();    
        //        break;
        //    default:
        //        Debug.LogError("player attack type not assigned");
        //        break;
        //}
        autoAttack = GetComponent<AutoAttack>();
        autoAttack.Initialize(player, attackOrigin);
    }
    void Update()
    {
        if (GameState.Instance.MenusOpen == 0 && player.UnitStatus != Unit.Status.Dazed && !player.CheckForSTFX<Stasis>())
        {
            CheckForMoveCommand();
            CheckForAttack();
            if (attackBuffered)
            {
                //Debug.Log(Vector2.Distance(transform.parent.position, attackBufferTarget.transform.position) <= stats.AttackRange);

                if (attackBufferTarget == null) // cancel attack if target has been destroyed
                {
                    attackBuffered = false;
                }
                else if (Vector2.Distance(transform.parent.position, attackBufferTarget.transform.position) <= player.AttackRange)
                {
                    Attack();
                    hasAutoed = true;
                }
                else if (Vector2.Distance(transform.parent.position, attackBufferTarget.transform.position) <= 0.1f) // break out in the worst case
                {
                    attackBuffered = false;
                }
                else
                {
                    targetPosition = attackBufferTarget.transform.position;
                }
            }

            if (!(player.UnitStatus == Unit.Status.Rooted))
            {
                transform.parent.position = Vector2.MoveTowards(transform.parent.position, targetPosition, player.MoveSpeed * Time.deltaTime);
            }


            if (!hasAutoed)
            {
                playerModel.GetComponent<FaceDirection>().DetermineFacingDirection(targetPosition);
            }
            hasAutoed = false;
        }
        attackIsUp = (attackCooldown == null);
    }

    public void MoveToCursor()
    {
        if (attackCoroutine != null)
        {
            StopCoroutine(attackCoroutine);
        }
        attackBuffered = false;
        targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }    
    
    public void SetTargetMovementPositionTo(Vector3 position)
    {
        targetPosition = position;
    }
    public void SetPlayerToPosition(Vector3 pos)
    {
        transform.parent.position = pos;
    }
    public void SetTargetPosition(Vector3 pos)
    {
        targetPosition = pos;
    }

    private void CheckForMoveCommand()
    {
        if (Input.GetMouseButtonDown(GameState.Instance.KeyMapInstance.MouseMap["move click"])
            && canInputMovementCommands)
        {
            MoveToCursor();
            Instantiate(moveClick, targetPosition, Quaternion.identity);
        }
        else if (Input.GetMouseButton(GameState.Instance.KeyMapInstance.MouseMap["move click"]) && canInputMovementCommands) {
            MoveToCursor(); // don't create moveclick effect if they are holding down right click as opposed to clicking and lifting off 
        }
    }
    private void CheckForAttack()
    {
        if (Input.GetKeyDown(GameState.Instance.KeyMapInstance.KeyMap["attack move"])
            && canInputMovementCommands)
        {
            Attack();
            Instantiate(attackClick, Camera.main.ScreenToWorldPoint(Input.mousePosition), Quaternion.identity);
            hasAutoed = true;
        }
    }
    public void Dash(Vector3 targetPosition, float speed)
    {
        StartCoroutine(DashCR(targetPosition, speed));
    }
    private IEnumerator DashCR(Vector3 targetPosition, float speed)
    {
        canInputMovementCommands = false;
        while (Vector3.Distance(transform.parent.position, targetPosition) > 0.2f) // threshold value for how close to intended target before resolving
        {
            while (GameState.Instance.MenusOpen > 0) yield return null;

            transform.parent.position = Vector2.MoveTowards(transform.parent.position, targetPosition, speed * Time.deltaTime * GameState.Instance.TimeScale);

            yield return null;
        }
        this.targetPosition = targetPosition;
        canInputMovementCommands = true;
    }

    private void Attack() {
        GameObject target = GameState.Instance.EnemySpawner.DetermineClosestTargetToCursor();
        if (target != null)
        {
            if (Vector3.Distance(target.transform.position, transform.parent.position) < player.AttackRange
    && attackIsUp == true)
            { //initiate auto attack

                attackCoroutine = StartCoroutine(Attack(target));
                targetPosition = transform.parent.position;
                playerModel.GetComponent<FaceDirection>().DetermineFacingDirection(target.transform.position);
                attackBuffered = false;
            }
            else // if out of range, buffer attack
            {
                targetPosition = target.transform.position;
                attackBuffered = true;
                attackBufferTarget = target;
            }
        }
        else
        {
            Debug.LogWarning("could not find target to autoattack");
        }
    }
    private float attackProgress;
    private IEnumerator Attack(GameObject target) {

        attackCooldown = gameObject.AddComponent<Cooldown>();
        attackCooldown.Duration = 1 / player.AttackSpeed;
        attackCooldown.Initiate();
        GameState gs = GameState.Instance;

        float timeElapsed = 0f;
        while (timeElapsed < 1/player.AttackSpeed) {
            while (gs.MenusOpen > 0) yield return null;
            attackProgress = timeElapsed / (1/player.AttackSpeed);
            yield return new WaitForSeconds(0.01f);
            timeElapsed += 0.01f;
        }

        float damage = player.Damage;
        bool isCrit = false;
        if (Random.Range(0f, 1f) <= player.CritChance) {
            damage *= player.CritDamage;
            isCrit = true;
        }

        attackCoroutine = null;

        if (target != null)
        {
            autoAttack.Attack(target.GetComponent<EnemyUnit>(), player.Damage, isCrit);
        }


    }
    public override float GetCooldownStatus()
    {
        if (attackCooldown == null)
        {
            return 1;
        }
        else
        {
            return attackProgress;
        }
    }
}
