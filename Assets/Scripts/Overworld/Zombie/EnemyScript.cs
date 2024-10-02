using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    //Declaring variables

    //The players transform and the Saveandload script on the player
    [SerializeField] Transform playerTransform;
    [SerializeField] SaveAndLoad saveAndLoad;

    //which encounter this enemy is
    public int encounter;
    
    //animator parameters
    private static int _animx = Animator.StringToHash("x");
    private static int _animy = Animator.StringToHash("y");
    private static int _animwalk = Animator.StringToHash("moving");

    //the enemies FOV angle and range
    public float fovAngle = 120;
    public float fovRange = 2.6f;

    //The enemies speed and lenght of walking cycle
    public int waitingTime = 5;
    public float speed = 1;

    //zombies animator and rigidbody
    private Animator animator;
    private Rigidbody2D zombieRigidbody;

    //variables for enemy AI
    private int walkingStage;
    private bool playerFound;
    private float timeWaited;

    //if the enemy is returning to it's partolroute
    private bool returning;

    
    
    private void Awake()
    {
        //gets components from zombie
        zombieRigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();


        //sets the animator to face left
        animator.SetFloat(_animx, -1);
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
                animator.SetBool(_animwalk, false);
            }
            else if (walkingStage == 1)
            {
                //move left and set animator to walk left
                zombieRigidbody.velocity = new Vector3(-1, 0, 0) * speed;
                animator.SetBool(_animwalk, true);
                animator.SetFloat(_animx, -1);
            }
            else if (walkingStage == 2)
            {
                //set velocity to 0 and stop the animator
                zombieRigidbody.velocity = Vector3.zero;
                animator.SetBool(_animwalk, false);
            }
            else if (walkingStage == 3)
            {
                //move right and set animator to walk right
                zombieRigidbody.velocity = new Vector3(1, 0, 0) * speed;
                animator.SetBool(_animwalk, true);
                animator.SetFloat(_animx, 1);
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
            float distance = Vector3.Distance(transform.position, playerTransform.position);
            Vector2 directionToTarget = (playerTransform.position - transform.position).normalized;
            

            //if player is still within the FOV set "distance" to distance to player and "directionToTarget" to the direction to the player
            if (IsTargetInsideFOV(playerTransform, fovRange + 1))
            {
                returning = false;
                timeWaited = 0;
            }
            //if the player is lost set returning to true, and the target is the starting position of it's patrolroute
            else
            {
                returning = true;
                //updates "timeWaited"
                timeWaited += Time.deltaTime;
            }

            //if the enemy is further than 0.5 units away from it's target and it's not returning or it's gone 2 seconds since it lost the player
            if (distance > 0.5f && (!returning || timeWaited > 2))
            {
                //Go towards target and make animator walk towards target
                zombieRigidbody.velocity = directionToTarget * speed;
                animator.SetBool(_animwalk, true);
                if (!(Mathf.Round(directionToTarget.x) == 0) || !(Mathf.Round(directionToTarget.y) == 0))
                {
                    if (Mathf.Round(directionToTarget.x) > directionToTarget.y)
                    {
                        
                    }
                }
                animator.SetFloat(_animx, Mathf.Round(directionToTarget.x));
                animator.SetFloat(_animy, Mathf.Round(directionToTarget.y));
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
                saveAndLoad.StartEncounter(encounter);
            }
            //if the enemy lost the player it stands still for 2 seconds
            else
            {
                //set velocity and animtaor to stand still
                zombieRigidbody.velocity = Vector3.zero;
                animator.SetBool(_animwalk, false);
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
        Vector2 lookDirection = new Vector2(animator.GetFloat(_animx), animator.GetFloat(_animy));

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