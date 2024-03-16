using System.Collections;
using System.Collections.Generic;
using RPG.Combat;
using RPG.Core;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Movement
{
public class Move : MonoBehaviour, IAction
{
    [SerializeField] float maxSpeed = 6f;
    NavMeshAgent navMeshAgent;
    Health health;
    void Update()
    {
        if (health.IsDead())
        {
            navMeshAgent.enabled = false;
        }
        UpdateAnimator();
    
    //Debug.DrawRay(lastRay.origin, lastRay.direction*100, Color.black);
    }
    void Start()
    {
        health = GetComponent<Health>();
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    public void MoveTo(Vector3 hit, float speedFraction)
    {
        navMeshAgent.speed = maxSpeed * speedFraction;
        navMeshAgent.destination = hit;
        navMeshAgent.isStopped = false;    

    }

    public void Cancel () 
    {
        navMeshAgent.isStopped = true;    
    }

    private void UpdateAnimator () 
    {
        Vector3 velocity = navMeshAgent.velocity;
        Vector3 localVelocity = transform.InverseTransformDirection(velocity);
        float speed = localVelocity.z;
        GetComponent<Animator>().SetFloat("ForwardSpeed" , speed);    
    }
    public void StartMoveAction (Vector3 hit, float speedFraction) 
    {
        GetComponent<Fighter>().Cancel();

        GetComponent<ActionScheduler>().StartAction(this);
        MoveTo(hit, speedFraction);

    }

}
}

