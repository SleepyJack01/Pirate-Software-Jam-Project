using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SearchState : EnemyState
{
    public SearchState(Enemy enemy) : base(enemy) {}
    private float time;
    private float delayRotationsTime;

    private Vector3[] randomDirections = new Vector3[3];
    private int randomDirectionIndex = 0;

    public override void OnStateEnter()
    {
        Debug.Log("Search State");

        enemy.agent.speed = enemy.searchSpeed;
        
        time = enemy.searchTime;
        delayRotationsTime = 1.0f;

        randomDirections[0] = enemy.transform.position + new Vector3(Random.Range(-10, 10), 0.0f, Random.Range(-10, 10));
        randomDirections[1] = enemy.transform.position + new Vector3(Random.Range(-10, 10), 0.0f, Random.Range(-10, 10));
        randomDirections[2] = enemy.transform.position + new Vector3(Random.Range(-10, 10), 0.0f, Random.Range(-10, 10));
    }

    public override void OnStateExit()
    {
        enemy.GetNearestWaypoint();
    }

    public override void OnStateUpdate()
    {
        enemy.agent.SetDestination(enemy.targetLastKnownPosition);

        if (enemy.agent.remainingDistance <= 0.1f)
        {
            time -= Time.deltaTime;

            if (delayRotationsTime <= 0.0f)
            {
                Vector3 direction = randomDirections[randomDirectionIndex] - enemy.transform.position;
                Quaternion LookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0.0f, direction.z));
                enemy.transform.rotation = Quaternion.Slerp(enemy.transform.rotation, LookRotation, Time.deltaTime * 5f);

                if (enemy.transform.rotation == LookRotation || Vector3.Angle(enemy.transform.forward, direction) < 1.0f)
                {
                    Debug.Log("Rotated to direction " + randomDirectionIndex);
                    delayRotationsTime = 1.0f;
                    randomDirectionIndex = (randomDirectionIndex + 1) % randomDirections.Length;
                }
            }
            else
            {
                delayRotationsTime -= Time.deltaTime;
            }
        }
        
        if (enemy.CanSeePlayer())
        {
            enemy.ChangeState(new ChaseState(enemy));
        }
        else if (time <= 0.0f)
        {
            enemy.ChangeState(new PatrolState(enemy));
        }
    }
}

