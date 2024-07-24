using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunState : EnemyState
{
    public StunState(Enemy enemy) : base(enemy) {}

    private float time;

    public override void OnStateEnter()
    {
        Debug.Log("Stun State");

        enemy.agent.speed = 0.0f;
        time = 3.0f;
    }

    public override void OnStateExit()
    {
        
    }

    public override void OnStateUpdate()
    {
        time -= Time.deltaTime;

        if (time <= 0.0f)
        {
            enemy.ChangeState(new PatrolState(enemy));
        }
    }
}
