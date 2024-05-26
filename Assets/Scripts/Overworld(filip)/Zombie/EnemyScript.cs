using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class EnemyScript : MonoBehaviour
{
    //Declaring variables

    //The players transform and the Saveandload script on the player
    [SerializeField] Transform playerTransform;
    [SerializeField] SaveAndLoad SaveAndLoad;

    //which encounter this enemy is
    public int encounter = 0;

    //the enemies FOV angle and range
    public float fovAngle = 60;
    public float fovRange = 2.6f;

    //The enemies speed and lenght of walking cycle
    public int waitingTime = 5;
    public float speed = 1;

    //zombies animator and rigidbody
    private Animator animator;
    private Rigidbody2D zombieRigidbody;

    //variables for enemy AI
    private int walkingStage = 0;
    private bool playerFound = false;
    private float timeWaited = 0;
    private Vector3 startingPosition = new Vector3 (4, 2, 0);

    //if the enemy is returning to it's partolroute
    private bool returning = false;

    private void Awake()
    {
        //gets components from zombie
        zombieRigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        //sets the animator to face left
        animator.SetFloat("x", -1);
    }

    private void Update()
    {
        //checks if player is inside the enemies fov
        if (IsTargetInsideFOV(playerTransform, fovRange))
        {
            playerFound = true;
        }

        //if player isn't found continue patrolling
        if (!playerFound)
        {
            //depending on walking stages is 1: stand, 2: walk left, 3:stand, 4:walk right
            if (walkingStage == 0)
            {
                //set velocity to 0 and stop the animator
                zombieRigidbody.velocity = Vector3.zero;
                animator.SetBool("moving", false);
            }
            else if (walkingStage == 1)
            {
                //move left and set animator to walk left
                zombieRigidbody.velocity = new Vector3(-1, 0, 0) * speed;
                animator.SetBool("moving", true);
                animator.SetFloat("x", -1);
            }
            else if (walkingStage == 2)
            {
                //set velocity to 0 and stop the animator
                zombieRigidbody.velocity = Vector3.zero;
                animator.SetBool("moving", false);
            }
            else if (walkingStage == 3)
            {
                //move right and set animator to walk right
                zombieRigidbody.velocity = new Vector3(1, 0, 0) * speed;
                animator.SetBool("moving", true);
                animator.SetFloat("x", 1);
            }
            else
            {
                //when walking stage 4 reset to 0
                walkingStage = 0;
            }

            //after "waitingTime" seconds go to next walking stage and reset "timeWaited"
            if (timeWaited > waitingTime)
            {
                walkingStage++;
                timeWaited = 0;
            }

            //update "timeWaited"
            timeWaited += Time.deltaTime;
        }
        //If player has been seen go towards him/her
        else
        {
            //the distance and direction to target
            float distance = 0;
            Vector2 directionToTarget = Vector2.zero;

            //if player is still within the FOV set "distance" to distance to player and "directionToTarget" to the direction to the player
            if (IsTargetInsideFOV(playerTransform, fovRange + 1))
            {
                distance = Vector3.Distance(transform.position, playerTransform.position);
                directionToTarget = (playerTransform.position - transform.position).normalized;
                returning = false;
                timeWaited = 0;
            }
            //if the player is lost set returning to true, and the target is the starting position of it's patrolroute
            else
            {
                distance = Vector3.Distance(transform.position,startingPosition);
                directionToTarget = (startingPosition - transform.position).normalized;
                returning = true;
                //updates "timeWaited"
                timeWaited += Time.deltaTime;
            }

            //if the enemy is further than 0.5 units away from it's target and it's not returning or it's gone 2 seconds since it lost the player
            if (distance > 0.5f && (!returning || timeWaited > 2))
            {
                //Go towards target and make animator walk towards target
                zombieRigidbody.velocity = directionToTarget * speed;
                animator.SetBool("moving", true);
                animator.SetFloat("x", directionToTarget.x);
                animator.SetFloat("y", directionToTarget.y);
            }
            //if the enemy has returned to it's startingPosition
            else if (returning && timeWaited > 2)
            { 
                walkingStage = 0;
                playerFound = false;
            }
            //if the enemy has reached the player
            else if (!returning)
            {
                //Start encounter
                SaveAndLoad.StartEncounter(encounter);
            }
            //if the enemy lost the player it stands still for 2 seconds
            else
            {
                //set velocity and animtaor to stand still
                zombieRigidbody.velocity = Vector3.zero;
                animator.SetBool("moving", false);
            }
        }
    }


    /// <summary>
    /// Checks if the "target" is within the enemies reach
    /// </summary>
    /// <param name="target">The taget to look for</param>
    /// <param name="range">The FOV range</param>
    /// <returns></returns>
    public bool IsTargetInsideFOV(Transform target, float range)
    {
        //the direction the enemy is looking
        Vector2 lookDirection = new Vector2(animator.GetFloat("x"), animator.GetFloat("y"));

        //the direction to the target
        Vector2 directionToTarget = (target.position - transform.position).normalized;

        //the angle to the target
        float angleToTarget = Vector2.Angle(lookDirection, directionToTarget);

        //if the angle to the target is less than "fovAngle/2"
        if (angleToTarget < fovAngle / 2)
        {
            //distance to the target
            float distance = Vector2.Distance(target.position, transform.position);

            //return true if the distance is less than the FOV range
            return distance < range;
        }
        return false;
    }

}