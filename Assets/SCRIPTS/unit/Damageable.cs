using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Damageable : MonoBehaviour
{
    private Unit unit;
    private InventoryManager inventoryManager;
    private UnitEffects unitEffects;
    private DamageNumbersManager dmgNumsManager;
    public void Initialize(Unit unit, UnitEffects unitEffects, InventoryManager inventoryManager)
    {
        this.unit = unit;
        this.unitEffects = unitEffects;
        this.inventoryManager = inventoryManager;
        dmgNumsManager = DamageNumbersManager.Instance;
    }
    public void TakeDamage(Attack attack)
    {
        if (!unit.CheckForSTFX<Stasis>())
        {
            if (attack.IsCrit)
            {
                GameState.Instance.Audio.PlaySound(ADFM.Sfx.OnHitCrit);
                attack.Damage *= GameState.Instance.Player.CritDamage;
            }
            else
            {
                GameState.Instance.Audio.PlaySound(ADFM.Sfx.OnHitNormal);
            }

            float extraDamage = 0;
            extraDamage = inventoryManager.ModifyDamageToUnits(unit, attack.Damage);

            if (unit is EnemyUnit)
            {
                extraDamage += inventoryManager.ModifyDamageToEnemies(unit, attack.Damage);
            }

            float modifiedDamage = attack.Damage + extraDamage;
            float roundedDmg = Mathf.Floor(modifiedDamage);

            if (unit is EnemyUnit)
            {
                GameState.Instance.Player.TotalDamageDealt += roundedDmg;
            }
            else if (unit is PlayerUnit)
            {
                GameState.Instance.Player.TotalDamageTaken += roundedDmg;
            }

            if (roundedDmg < 0)
            {
                Debug.LogWarning("negative damage value thrown out");
                return;
            }
            else
            {
                if (!unit.CheckForSTFX<Undamageable>())
                {
                    if (unit.Shield > 0)
                    {
                        unit.Shield -= roundedDmg;
                    }
                    else
                    {
                        unit.CurrentHealth -= roundedDmg;
                    }
                }
                else
                {
                    dmgNumsManager.CreateMessage("immune!", transform.position);
                }
            }

            if (attack.statusEffectList.Count > 0)
            {
                foreach (Unit.STFX stfx in attack.statusEffectList)
                {
                    unit.ApplySTFX(stfx);
                }
            }


            //FlashOnDamage();
            if (!attack.statusEffectList.Contains(Unit.STFX.Daze) &&
                !attack.statusEffectList.Contains(Unit.STFX.Root)
                )
            {
                unitEffects.Damaged();
            }
            dmgNumsManager.CreateMessage(roundedDmg, attack.IsCrit, transform.position);
            Instantiate(GameAssets.Instance.HitParticles, gameObject.transform.position + new Vector3(0, 2, 0), Quaternion.identity);

        }
    }
    public virtual void Kill(bool executed = false)
    {
        Instantiate(GameAssets.Instance.DeathParticles, gameObject.transform.position, Quaternion.identity);
        if (executed)
        {
            dmgNumsManager.CreateMessage("executed!", transform.position);
        }
        StartCoroutine(Kill());
    }

    private IEnumerator Kill()
    {
        unit.isDying = true;
        float timeElapsed = 0f;
        float duration = 0.5f;
        GameState gs = GameState.Instance;
        SpriteRenderer model;
        if (unit is PlayerUnit)
        {
            PlayerUnit p = (PlayerUnit)unit;
            model = p.Model;
        }
        else if (unit is EnemyUnit) {
            EnemyUnit e = (EnemyUnit)unit;
            model = e.model;
        }
        else
        {
            yield break;
        }

        Quaternion initialRotation = model.gameObject.transform.rotation;
        Quaternion targetRotation;

        float deathRotation = 20f;

        // Determine target rotation based on flipX
        if (model.flipX)
        {
            targetRotation = Quaternion.Euler(0, 0, deathRotation);
        }
        else
        {
            targetRotation = Quaternion.Euler(0, 0, -deathRotation);
        }

        Color deadRed;
        ColorUtility.TryParseHtmlString("#AE0000", out deadRed);
        model.color = deadRed;
        Transform pivot = model.gameObject.transform.parent;

        while (timeElapsed < duration)
        {
            while (gs.MenusOpen > 0) yield return null;

            // Update elapsed time
            timeElapsed += Time.deltaTime;

            // Lerp the rotation from the initial to the target rotation
            pivot.rotation = Quaternion.Lerp(
                initialRotation,
                targetRotation,
                timeElapsed / duration
            );

            yield return null; // Wait until the next frame
        }

        // Ensure final rotation is set exactly to the target value
        pivot.rotation = targetRotation;

        Destroy(gameObject);
    }

    protected void FlashOnDamage()
    {
        SpriteRenderer sprite;
        if (unit.FlyingUnit)
        {
            sprite = transform.Find("model").GetComponent<SpriteRenderer>();
        }
        else // not a flying unit.. needs to be specified later if more types are added and the hierarchy looks different for them
        {
            sprite = transform.Find("pivot").Find("model").GetComponent<SpriteRenderer>();
        }

        Shader guiText = Shader.Find("GUI/Text Shader");
        Shader spriteDefault = Shader.Find("Sprites/Default");
        //Debug.Log(guiText == null);
        //Debug.Log(spriteDefault == null);
        StartCoroutine(FlashOnDmgCR(sprite, 0.13f, guiText, spriteDefault));
    }

    protected virtual IEnumerator FlashOnDmgCR(SpriteRenderer sprite, float duration, Shader guiText, Shader spriteDefault)
    {
        if (unit is EnemyUnit) sprite.GetComponent<OutlineOnHover>().outlineFunctionEnabled = false;
        sprite.material.shader = guiText;

        float timeElapsed = 0f;
        while (timeElapsed < duration)
        {
            while (GameState.Instance.MenusOpen > 0) yield return null;

            float deltaTime = Time.deltaTime * GameState.Instance.TimeScale;
            timeElapsed += deltaTime;
            yield return null;
        }

        sprite.material.shader = spriteDefault;
        if (unit is EnemyUnit) sprite.GetComponent<OutlineOnHover>().outlineFunctionEnabled = true;

        if (unit.CurrentHealth <= 0)
        {
            unit.Kill();
        }
        else
        {
            EventManager.UnitTookDamageEvent(unit);
        }
    }

}
