using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState :  EnemyState
{
    public PatrolState(Enemy enemy) : base(enemy) {}

    private float time;

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
        // Check if the enemy has reached the waypoint
        if (enemy.agent.remainingDistance <= 0.1f)
        {
            // Wait at the waypoint for the specified time
            if (time <= 0)
            {
                time = enemy.waitTimes[enemy.waypointIndex];
            }
            else
            {
                time -= Time.deltaTime;
            }

            // If the time has elapsed, move to the next waypoint
            if (time <= 0)
            {
                enemy.waypointIndex = (enemy.waypointIndex + 1) % enemy.waypoints.Length;
                enemy.agent.SetDestination(enemy.waypoints[enemy.waypointIndex].position);
            }
        }

        if (enemy.CanSeePlayer())
        {
            enemy.ChangeState(new SpottedPlayerState(enemy));
        }
    }
}
