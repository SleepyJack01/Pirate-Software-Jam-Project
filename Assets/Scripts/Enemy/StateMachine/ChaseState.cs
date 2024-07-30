using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseState : EnemyState
{
    public ChaseState(Enemy enemy) : base(enemy) {}

    public override void OnStateEnter()
    {
        Debug.Log("Chase State");

        enemy.agent.speed = enemy.chaseSpeed;
    }

    public override void OnStateExit()
    {
        
    }

    public override void OnStateUpdate()
    {
        enemy.agent.SetDestination(enemy.target);


        if (PlayerManager.isDead)
        {
            enemy.ChangeState(new PatrolState(enemy));
        }
        else if (!enemy.CanSeePlayer())
        {
            enemy.agent.SetDestination(enemy.targetLastKnownPosition);

            if (enemy.agent.remainingDistance <= 0.1f)
            {
                enemy.ChangeState(new SearchState(enemy));
            }
        }
    }
}
