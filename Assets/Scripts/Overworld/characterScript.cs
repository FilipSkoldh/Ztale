using UnityEngine.InputSystem;
using UnityEngine;

public class CharacterScript : MonoBehaviour
{
    //Declaring variables
    //Floats needed to make sprinting and crouching
    [SerializeField] private float speed = 1;
    [SerializeField] private float stamina = 10;
    [SerializeField] private float sprintCooldown = 1;

    //All different inputs
    [SerializeField] private InputActionProperty move;
    [SerializeField] private InputActionProperty sprint;
    [SerializeField] private InputActionProperty crouch;
    [SerializeField] private InputActionProperty interact;

    //The canvas the textbox appears in and the inventory GUI
    [SerializeField] private Canvas textboxCanvas;
    [SerializeField] private GameObject inventoryGUI;
    [SerializeField] private GameObject escMenu;

    //Other scripts
    private InkScript inkScript;

    //Did i sprint last frame?
    private bool sprintedLastFrame;

    //Duhhh
    private Rigidbody2D playerRigidbody;
    private Animator playerAnimator;
    private Transform playerTransform;


    //Only runs when the script 
    private void Awake()
    {
        //Gets the components that are on the player
        playerRigidbody = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();
        playerTransform = GetComponent<Transform>();
        inkScript = GetComponent<InkScript>();

        //for now loads the variables like hp and max hp
    }

    // Update is called once per frame
    void Update()
    {
        //Only be able to move if the textbox isn't active and you inventory isn't active
        if (!(textboxCanvas.transform.childCount != 0 || inventoryGUI.activeSelf || escMenu.activeSelf))
        {
            //Start with resetting from eventual crouch or sprint
            playerTransform.localScale = new Vector3(1, 1, 1);
            speed = 1;

            //Read the inputs and put the into "movevector"
            Vector2 movevector = move.action.ReadValue<Vector2>();

            //If the player is moving activate the animator and tell it where the player is moving
            if (movevector != Vector2.zero)
            {
                playerAnimator.SetBool("moving", true);
                playerAnimator.SetFloat("x", movevector.x);
                playerAnimator.SetFloat("y", movevector.y);
            }
            //Else turn the animator off
            else
            {
                playerAnimator.SetBool("moving", false);
            }

            //If the crouch button is pressed activate the amazing crouch animation and set the speed to half
            if (crouch.action.IsPressed())
            {
                playerTransform.localScale = new Vector3(1, 0.4f, 1);
                speed = 0.5f;
            }

            //Sprint only if the sprint button is pressed, the crouch button isn't, you have over 0 stamina, u are moving, and there isn't a cooldown except when you were sprinting last frame then it ignores the cooldown
            if (sprint.action.IsPressed() && !crouch.action.IsPressed() && stamina > 0 && movevector != Vector2.zero && (sprintCooldown < 0.1 || sprintedLastFrame))
            {
                //Sprints by setting the speed to 2, consuming 1 stamina per second, setting the cooldown to 1 and setting the sprinted last frame to true
                speed = 2;
                stamina -= Time.deltaTime * 1;
                sprintCooldown = 1;
                sprintedLastFrame = true;
            }
            //If the character isn't sprinting regen stamina and and count down the cooldown
            else
            {
                //If the cooldown is up remove it in 1 sec and set the sprintedLastFrame to false
                if (sprintCooldown > 0)
                {
                    sprintCooldown -= Time.deltaTime * 1;
                    sprintedLastFrame = false;
                }
                //If the cooldown is gone begin regenerating stamina
                else if (stamina < 10)
                {
                    stamina += Time.deltaTime * 1.5f;
                }
            }

            //Set the animator to animate at the speed you're walking
            playerAnimator.speed = speed;
            //Set the players velocity to where you are pressing * the speed term
            playerRigidbody.velocity = movevector * speed;

            //If you pressed the interact button boxcast to see what's infront of the player
            if (interact.action.WasPressedThisFrame())
            {
                //Boxcasting infront of the player and checks if it's empty
                Transform cast = Physics2D.BoxCast(transform.position, new Vector2(0.5f, 0.5f), 0, new Vector2(playerAnimator.GetFloat("x"), playerAnimator.GetFloat("y")), 0.4f, 8).transform;
                if (cast != null)
                {
                    //if the GameObject had a Ink asset give it to the InkScript and tells it to start displaying it
                    if (cast.GetComponent<SimpleInkStorage>() != null)
                    {
                        inkScript.inkJSONAsset = cast.GetComponent<SimpleInkStorage>().inkStorage;
                        inkScript.StartStory();
                    }
                }
            }
        }
        //if the inventory or textbox was open set the animator and velocity to not moving
        else
        {
            playerRigidbody.velocity = Vector3.zero;
            playerAnimator.SetBool("moving", false);
        }
    }
}
