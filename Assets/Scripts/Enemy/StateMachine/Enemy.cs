using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [Header("State Controls")]
    public EnemyState currentState;
    public EnemyState previousState;
    public Animator animator;
    public float stunTimer = 0f;
    public float stunDuration = 3.0f;

    [Header("Movement Variables")]
    public float walkSpeed = 3.5f;
    public float searchSpeed = 4.5f;
    public float chaseSpeed = 6.0f;

    [Header("Navigation")]
    [HideInInspector]
    public NavMeshAgent agent;
    public Transform[] waypoints;
    public float[] waitTimes;
    [HideInInspector]
    public int waypointIndex;
    public float searchTime = 6.0f;
    public float pauseTime = 1.5f;
    public float updateTargetDelay = 0.5f;
    private float time;

    
    [Header("Player Detection")]
    [HideInInspector]
    public Transform player;
    private FieldOfView fieldOfView;
    [HideInInspector]
    public Vector3 target;
    [HideInInspector]
    public Vector3 targetLastKnownPosition;

    [Header("Animation Variables")]
    private float xMovement;
    private float zMovement;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        fieldOfView = GetComponent<FieldOfView>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        animator = GetComponentInChildren<Animator>();

        ChangeState(new PatrolState(this));
    }

    public void ChangeState(EnemyState newState)
    {
        if (currentState != null)
        {
            currentState.OnStateExit();
        }

        previousState = currentState;
        currentState = newState;
        currentState.OnStateEnter();
        Debug.Log($"Current state: {currentState.ToString()}");
    }

    private void Update()
    {
        if (currentState != null)
        {
            currentState.OnStateUpdate();
        }

        UpdateTarget();
        SetAnimatorSpeed();
    }
    
    public void SetSpeed(float speed)
    {
        agent.speed = speed;
    }

    public bool CanSeePlayer()
    {
        if (fieldOfView.visibleTargets.Count > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void UpdateTarget()
    {
        if (CanSeePlayer())
        {
            time = updateTargetDelay;
            target = player.position;
            targetLastKnownPosition = player.position;
        }
        else
        {
            time -= Time.deltaTime;

            if (time <= 0)
            {
                target = targetLastKnownPosition;
            }
            else
            {
                targetLastKnownPosition = player.position;
            }
        }
    }

    public void GetNearestWaypoint()
    {
        float distance = Mathf.Infinity;
        int index = 0;

        for (int i = 0; i < waypoints.Length; i++)
        {
            float newDistance = Vector3.Distance(transform.position, waypoints[i].position);

            if (newDistance < distance)
            {
                distance = newDistance;
                index = i;
            }
        }

        waypointIndex = index;
    }

    public void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("StunPotion"))
        {
            ChangeState(new StunState(this));
        }

    }

    private void SetAnimatorSpeed()
    {
        Vector3 velocity = agent.velocity;
        Vector3 localVelocity = transform.InverseTransformDirection(velocity);

        xMovement = localVelocity.x;
        zMovement = localVelocity.z;

        animator.SetFloat("xMovement", xMovement);
        animator.SetFloat("zMovement", zMovement);
    }
}
