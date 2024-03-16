using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Core;
using RPG.Movement;
using UnityEngine;
using UnityEngine.PlayerLoop;


namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction
    {
        Health targetObject;
        [SerializeField] float timeBetweenAttacks = 1f;

        [SerializeField] float timeSinceLastAttack;
        [SerializeField] float stopThreshold = 0.3f;

        [SerializeField] Transform rightHandTransform = null;
        [SerializeField] Transform leftHandTransform = null;
        [SerializeField] Weapon defaultWeapon=null;

        void Start()
        {
           
            SpawnWeapon(defaultWeapon);

        }

        public void SpawnWeapon(Weapon weapon)
        {
            defaultWeapon = weapon;
            Animator animator = GetComponent<Animator>();

            defaultWeapon.Spawn(rightHandTransform,leftHandTransform,animator);
        }


        void Update()
        {
            timeSinceLastAttack += Time.deltaTime;

            if(targetObject != null)
            {
                if (targetObject.IsDead() == true)
                {
                    GetComponent<Animator>().ResetTrigger("Attack");
                    Cancel();
                    return;
                }
                if (GetIsInRange() == false)
                {
                    GetComponent<Move>().MoveTo(targetObject.transform.position, 1f);
                }
                else
                {
                   
                    AttackAnim();
                    GetComponent<Move>().Cancel();
                }
            } 
            
        }

        private void AttackAnim()
        {
            transform.LookAt(targetObject.transform);
            if (Mathf.Abs(GetComponent<Animator>().GetFloat("ForwardSpeed")) < stopThreshold )
                {
                    GetComponent<Animator>().SetFloat("ForwardSpeed", 0f);
                }
            if(timeSinceLastAttack > timeBetweenAttacks)
            {
                TriggerAttack();
                timeSinceLastAttack = 0;
            }

           
        }
        public bool CanAttack(GameObject combatTarget)
        {
            if (combatTarget == null)
            {
                return false;
            }
            Health healthToTest = GetComponent<Health>();
            return healthToTest != null && !healthToTest.IsDead();
        }

        private void TriggerAttack()
        {
            GetComponent<Animator>().ResetTrigger("stopAttack");
            GetComponent<Animator>().SetTrigger("Attack");
        }

        private bool GetIsInRange()
        {
            return Vector3.Distance(transform.position, targetObject.transform.position) < defaultWeapon.GetRange();
        }

        public void Attack (GameObject target) {
            targetObject = target.GetComponent<Health>();
            GetComponent<ActionScheduler>().StartAction(this);
        }
        public void Cancel ()
        {
            StopAttack();
            targetObject = null;
        }

        private void StopAttack()
        {
            GetComponent<Animator>().ResetTrigger("Attack");
            GetComponent<Animator>().SetTrigger("stopAttack");
        }

        void Hit()
        {
            if(targetObject == null) 
            {
                return;
            }
            if (defaultWeapon.HasProjectile() == true)
            {
                defaultWeapon.LunchProjectile(rightHandTransform,leftHandTransform,targetObject);
            }
            else
            {
            targetObject.TakeDamage(defaultWeapon.GetDamage());
            }
            Health health = targetObject.GetComponent<Health>();

        }
    }
}

