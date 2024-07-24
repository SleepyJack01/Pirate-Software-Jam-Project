using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpottedPlayerState : EnemyState
{
    private float time;

    public SpottedPlayerState(Enemy enemy) : base(enemy) {}

    public override void OnStateEnter()
    {
        Debug.Log("Spotted Player State");

        enemy.agent.speed = 0.0f;

        time = enemy.pauseTime;
    }

    public override void OnStateExit()
    {

    }

    public override void OnStateUpdate()
    {
        time -= Time.deltaTime;
        
        Vector3 direction = enemy.target+ - enemy.transform.position;
        Quaternion LookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0.0f, direction.z));
        enemy.transform.rotation = Quaternion.Slerp(enemy.transform.rotation, LookRotation, Time.deltaTime * 5f);

        enemy.agent.SetDestination(enemy.target);

        if (!enemy.CanSeePlayer() && time <= 0.0f)
        {
            enemy.ChangeState(new SearchState(enemy));
        }
        else if (enemy.CanSeePlayer() && time <= 0.0f)
        {
            enemy.ChangeState(new ChaseState(enemy));
        }
        
    }
}
