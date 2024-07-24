using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState :  EnemyState
{
    public PatrolState(Enemy enemy) : base(enemy) {}

    public override void OnStateEnter()
    {
        Debug.Log("Patrol State");

        enemy.agent.speed = enemy.walkSpeed;
    }

    public override void OnStateExit()
    {
        
    }

    public override void OnStateUpdate()
    {

        if (enemy.agent.remainingDistance <= 0.1f)
        {
            enemy.waypointIndex = (enemy.waypointIndex + 1) % enemy.waypoints.Length;
            enemy.agent.SetDestination(enemy.waypoints[enemy.waypointIndex].position);
        }

        if (enemy.CanSeePlayer())
        {
            enemy.ChangeState(new SpottedPlayerState(enemy));
        }
    }
}
