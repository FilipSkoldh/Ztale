using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public class Characterscript : MonoBehaviour
{
    ///Declaring shit
    public float speed = 1;
    public float stamina = 10;
    public float staminacooldown = 1;
    private bool sprint;
    private bool crouch;
    private bool sprintLF = false;
    private Animator anim;
    private InputAction moveaction;
    private InputAction sprintaction;
    private InputAction crouchaction;
    private Rigidbody2D rb;
    private Transform trans;
    public InputActionAsset actions;

    void OnEnable()
    {
        actions.FindActionMap("char").Enable();
    }

    private void Awake()
    {
        moveaction = actions.FindActionMap("char").FindAction("move");
        sprintaction = actions.FindActionMap("char").FindAction("sprint");
        crouchaction = actions.FindActionMap("char").FindAction("crouch");
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        trans = GetComponent<Transform>();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        sprint = sprintaction.IsPressed();
        crouch = crouchaction.IsPressed();

        trans.localScale = new Vector3(1, 1, 1);
        speed = 1;
        
        Vector2 movevector = moveaction.ReadValue<Vector2>();
        if (movevector != Vector2.zero)
        {
            anim.SetBool("moving", true);
            anim.SetFloat("x", movevector.x);
            anim.SetFloat("y", movevector.y);
        }
        else
        {
            anim.SetBool("moving", false);
        }
        if (crouch)
        {
            trans.localScale = new Vector3(1, 0.4f, 1);
            speed = 0.5f;
        }

        if (sprint && !crouch && stamina > 0 && movevector != Vector2.zero && (staminacooldown < 0.1 || sprintLF))
        {
            speed = 2;
            stamina = stamina - Time.deltaTime * 1;
            staminacooldown = 1;
            sprintLF = true;
            anim.speed = speed;
        }
        else
        {
            anim.speed = speed;
            if (staminacooldown > 0)
            {
                staminacooldown = staminacooldown - Time.deltaTime * 1;
                sprintLF = false;
            }
            else if (stamina < 10)
            {
                stamina = stamina + Time.deltaTime * 1.5f;
            }
        }
        rb.velocity = movevector * speed;
    }
}
