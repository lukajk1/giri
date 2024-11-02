using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class EnemyAttack : MonoBehaviour
{
    public const float HITBOX_ACTIVE_LENGTH = 0.3f;

    protected void DetermineAttackColor(SpriteRenderer sprite, AttackScriptable data) {

        Color red;
        Color blue;
        Color purple;

        ColorUtility.TryParseHtmlString("#ff5252", out red);
        ColorUtility.TryParseHtmlString("#48d2ff", out blue);
        ColorUtility.TryParseHtmlString("#c741ff", out purple);

        if (data.StatusEffects.Length > 0)
        {

            if (data.StatusEffects.Contains(Unit.STFX.Daze) ||
                data.StatusEffects.Contains(Unit.STFX.Root)
                )
            {
                sprite.color = blue;
            }
            else if (data.StatusEffects.Contains(Unit.STFX.Bleed))
            {
                sprite.color = purple;
            }
            else
            {
                sprite.color = red;
            }
        }
        else
        {
            sprite.color = red;
        }
    }
}
