using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class AI : AIPath
{
    DefEnemy defEnemy;
    BlindEnemy blindEnemy;

    protected override void Awake()
    {
        base.Awake();

        defEnemy = GetComponent<DefEnemy>();
        blindEnemy = GetComponent<BlindEnemy>();
    }

    protected override void OnTargetReached()
    {
        base.OnTargetReached();

        if (defEnemy != null)
        {
            defEnemy.OnDestinationReached();
        }
        if (blindEnemy != null)
        {
            blindEnemy.OnDestinationReached();
        }
    }

}
