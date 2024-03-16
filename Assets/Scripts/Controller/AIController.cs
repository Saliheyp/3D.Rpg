using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using RPG.Combat;
using RPG.Core;
using RPG.Movement;
using UnityEngine;


namespace RPG.Controller
{
public class AIController : MonoBehaviour
{
    Move move;
    GameObject player;
    Vector3 baseLocation;
    Fighter fighter;
    Health health;
    [SerializeField] float chaseDistance = 10f;
    [Range(0,1)]
    [SerializeField] float patrolSpeedFraction = 0.2f;
    [SerializeField] float shoutDistance = 5f;

    [SerializeField] float suspicionTime = 5f;
    [SerializeField] float aggroCooldownTime = 5f;
    [SerializeField] float wayPointTolerance = 2f;
    float timeSinceLastSawPlayer;
    [SerializeField]float timeSinceArrivedWayPoint;

    [SerializeField] PatrolPath patrolPath;
    [SerializeField] float wayPointLifeTime = 2f;
    int currentWayPointIndex = 0;
    float timeSinceAggravate = Mathf.Infinity;

    void Start()
    {
        player = GameObject.FindWithTag("Player");
        fighter = GetComponent<Fighter>();
        health = GetComponent<Health>();
        baseLocation = transform.position;
        move = GetComponent<Move>();
        
    }

    void Update()
        {
            if (health.IsDead())
            {
                return;
            }
            if (IsAggravate() && fighter.CanAttack(player))
            {
                fighter.Attack(player);
                timeSinceLastSawPlayer = 0;
                AggravateNearbyEnemies();
            }
            else if (timeSinceLastSawPlayer < suspicionTime)
            {
                GetComponent<ActionScheduler>().CancelCurrentAction();
            }
            else
            {
                Vector3 nextPosition = baseLocation;
                if (patrolPath != null)
                {
                    if (AtWaypoint())
                    {
                        timeSinceArrivedWayPoint = 0f;
                        CycleWayPoint();
                    }
                    nextPosition = GetNextWayPoint();
                }
                if (timeSinceArrivedWayPoint > wayPointLifeTime)
                {
                    move.StartMoveAction(nextPosition, patrolSpeedFraction);
                }
            }
            timeSinceLastSawPlayer += Time.deltaTime;
            timeSinceArrivedWayPoint += Time.deltaTime;
            timeSinceAggravate += Time.deltaTime;
        }

        private void AggravateNearbyEnemies()
        {
            RaycastHit[] hits = Physics.SphereCastAll(transform.position , shoutDistance ,Vector3.up,0);
            foreach (RaycastHit hit in hits)
            {
                AIController ai = hit.collider.GetComponent<AIController>();
                if (ai == null) continue;
                
                ai.Aggravate();
                    
                
            }
        }

        private bool IsAggravate()
        {
            return DistanceToPlayer() < chaseDistance || timeSinceAggravate < aggroCooldownTime;
        }

        public void Aggravate () 
        {
            timeSinceAggravate = 0;
        }

        private Vector3 GetNextWayPoint()
        {
            return patrolPath.GetWayPointPosition(currentWayPointIndex);
        }

        private void CycleWayPoint()
        {
            currentWayPointIndex = patrolPath.GetNextIndex(currentWayPointIndex);
        }

        private bool AtWaypoint()
        {
            float distanceWayPoint = Vector3.Distance(transform.position, GetNextWayPoint());
            return distanceWayPoint < wayPointTolerance;
        }

        private float DistanceToPlayer()
        {
           return Vector3.Distance(player.transform.position, transform.position);
        }
        void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, chaseDistance);
        }
    }
}
