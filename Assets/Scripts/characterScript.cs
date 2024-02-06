using UnityEngine.InputSystem;
using UnityEngine;

public class characterScript : MonoBehaviour
{
    ///Declaring shit
    public float speed = 1;
    public float stamina = 10;
    public float staminacooldown = 1;
    private bool sprintLF = false;
    private Animator anim;
    public InputActionProperty move;
    public InputActionProperty sprint;
    public InputActionProperty crouch;
    private Rigidbody2D rb;
    private Transform trans;




    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        trans = GetComponent<Transform>();
    }


    // Update is called once per frame
    void Update()
    {

        trans.localScale = new Vector3(1, 1, 1);
        speed = 1;

        Vector2 movevector = move.action.ReadValue<Vector2>();

        Debug.Log(movevector);
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
        if (crouch.action.IsPressed())
        {
            trans.localScale = new Vector3(1, 0.4f, 1);
            speed = 0.5f;
        }

        if (sprint.action.IsPressed() && !crouch.action.IsPressed() && stamina > 0 && movevector != Vector2.zero && (staminacooldown < 0.1 || sprintLF))
        {
            speed = 2;
            stamina -= Time.deltaTime * 1;
            staminacooldown = 1;
            sprintLF = true;
            anim.speed = speed;
        }
        else
        {
            anim.speed = speed;
            if (staminacooldown > 0)
            {
                staminacooldown -= Time.deltaTime * 1;
                sprintLF = false;
            }
            else if (stamina < 10)
            {
                stamina += Time.deltaTime * 1.5f;
            }
        }
        rb.velocity = movevector * speed;
    }
}
