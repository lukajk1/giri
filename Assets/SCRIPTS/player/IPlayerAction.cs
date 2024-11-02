using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayerAction
{
    void Propagate(PlayerController player);
}

public class PlayerMove : IPlayerAction
{
    public void Propagate(PlayerController player)
    {

    }
}
public class UseMovementAbility : IPlayerAction
{
    public void Propagate(PlayerController player)
    {

    }
}
public class PlayerAutoAttack : IPlayerAction
{
    public void Propagate(PlayerController player)
    {

    }
}