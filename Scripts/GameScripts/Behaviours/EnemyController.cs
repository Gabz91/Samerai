using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GenericScripts;

namespace GameScripts
{
    public class EnemyController : MonoBehaviour
    {
        public NPC foe;
        public static Transform target;
        Animator animator;
        bool isInFront;
        bool isFacingRight;
        Quaternion facingDirection;
        float timeSinceLastAttack;
        Vector3 distanceFromTarget;



        /// <summary>
        /// Holds a reference to the weapon used by this foe.
        /// </summary>
        Weapon equippedWeapon;

        SpriteRenderer[] renderers;

        PoolOfObjects kunaiPools;

        void Start()
        {
            if (target == null)
                target = ResourceManager.Instance.playerTrans;
            animator = GetComponentInChildren<Animator>();
            facingDirection = transform.rotation;
            renderers = GetComponentsInChildren<SpriteRenderer>();
            kunaiPools = new PoolOfObjects(ResourceManager.Instance.kunaisPool, ResourceManager.Instance.kunaiPrefab);
        }

        void Update()
        {
            distanceFromTarget = target.position - transform.position;
            //Get the difference.
            //If target is not within range (change it to match the attack range of the unit)
            if (distanceFromTarget.magnitude > foe.attackRange)
            {
                distanceFromTarget = distanceFromTarget.normalized;
                Vector3 newTargetPos = target.position;
                newTargetPos.y = transform.position.y;
                transform.position = Vector3.MoveTowards(transform.position, newTargetPos, foe.Speed * Time.deltaTime);
            }
            else
            {
                BasicAttack();
            }
            //If the foe is in front of the player
            if (distanceFromTarget.z > 0 && !isInFront)
            {
                isInFront = true;
                AdjustZOrder();
            }
            else if (distanceFromTarget.z < 0 && isInFront)
            {
                isInFront = false;
                AdjustZOrder();
            }
            Flip();
            
        }
        
        void Flip()
        {
            //If the NPC is on the player left and is facing left
            if (transform.position.x < target.position.x && facingDirection.y == 180)
            {
                isFacingRight = true;
                facingDirection.y = 0;
                transform.rotation = facingDirection;
            }
            else if (transform.position.x > target.position.x && facingDirection.y == 0)
            {
                isFacingRight = false;
                facingDirection.y = 180;
                transform.rotation = facingDirection;
            }
        }

        void AdjustZOrder()
        {
            foreach (SpriteRenderer sr in renderers)
            {
                if (isInFront)
                {
                    sr.sortingLayerName = "Player";
                }
                else
                {
                    sr.sortingLayerName = "Default";
                }
            }
        }

        void BasicAttack()
        {
            switch (foe.type)
            {
                case NPCType.MELEE_UNIT:
                    if (distanceFromTarget.z <= 0.2f)
                    {
                        animator.SetTrigger("Attack1");
                    }
                    break;
                case NPCType.RANGED_UNIT:
                    if (Time.time - timeSinceLastAttack > 8)
                    {
                        Vector3 direction = (target.position - transform.position).normalized;
                        GameObject newKunai = kunaiPools.GetObject();

                        newKunai.GetComponent<BoxCollider>().isTrigger = true;
                        newKunai.GetComponent<Rigidbody>().useGravity = false;

                        newKunai.transform.position = transform.position;
                        newKunai.transform.right = direction;
                        newKunai.GetComponent<Rigidbody>().velocity = direction * 5;
                        timeSinceLastAttack = Time.time;
                    }
                    break;
            }
        }
    }
}
