using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IOnDeathBehavior 
{
    void Initialize(Unit unit);
    void MoveNext();
}
