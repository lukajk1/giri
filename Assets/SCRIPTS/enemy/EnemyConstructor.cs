using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyConstructor : MonoBehaviour
{
    [HideInInspector] public EnemyData Data;
    [SerializeField] private SpriteRenderer model;
    public EnemyUnit Initialize()
    {
        EnemyUnit unit = gameObject.AddComponent<EnemyUnit>();
        if (model == null)
        {
            Debug.LogError($"{Data.unitVanityName} is missing a model assignment!");
            return unit;
        }
        else if (Data.attackData == null)
        {
            Debug.LogError($"{Data.unitVanityName} is missing an AttackScriptable object!");
            return unit;
        }

        //unit.model = model;
        //unit.Data = Data;
        unit.Initialize(Data, model);

        if (Data.FlyingUnit)
        {
            FloatingMotion bob = model.gameObject.AddComponent<FloatingMotion>();
            bob.bobbingHeight = Data.bobbingHeight;
            bob.bobbingSpeed = Data.bobbingSpeed;
        }
        else
        {
            WalkingMotion walk = model.gameObject.AddComponent<WalkingMotion>();
            walk.Initialize(Data);
        }

        EnemyController controller = gameObject.AddComponent<EnemyController>();
        controller.Initialize(model, Data.attackData, Data);

        HB_Setup hp = gameObject.AddComponent<HB_Setup>();
        hp.Initialize(unit, Data);

        IEnemyBehavior[] behaviorList = gameObject.GetComponents<IEnemyBehavior>();
        foreach (IEnemyBehavior behavior in behaviorList)
        {
            behavior.Initialize(unit, Data);
        }

        switch (Data.AttackStyle)
        {
            case EnemyData.AttackType.RangedCaster:
                controller.attack = gameObject.AddComponent<RangedCasterAtk>();
                break;
            case EnemyData.AttackType.RangedOnPlayer:
                controller.attack = gameObject.AddComponent<RangedOnPlayerAtk>();
                break;
            case EnemyData.AttackType.Multi:
                controller.attack = gameObject.AddComponent<MultiFieldAtk>();
                break;

            // melee attack start here
            case EnemyData.AttackType.AimedMelee:
                controller.attack = gameObject.AddComponent<AimedMeleeAtk>();
                controller.attack.parentEnemyObject = gameObject;
                break;
            case EnemyData.AttackType.OnSelf:
                controller.attack = gameObject.AddComponent<OnSelfAtk>();
                controller.attack.parentEnemyObject = gameObject;
                break;
            case EnemyData.AttackType.Kamikaze:
                controller.attack = gameObject.AddComponent<KamikazeAtk>();
                controller.attack.parentEnemyObject = gameObject;
                break;
        }
        controller.attack.data = Data;
        return unit;
    }
}

