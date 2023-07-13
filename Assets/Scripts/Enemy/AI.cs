using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class AI : AIPath
{
    Enemy enemy;

    protected override void Awake()
    {
        base.Awake();

        enemy = GetComponent<Enemy>();
    }

    protected override void OnTargetReached()
    {
        base.OnTargetReached();

        enemy.OnDestinationReached();
    }

}
