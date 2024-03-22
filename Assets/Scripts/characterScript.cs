using UnityEngine.InputSystem;
using UnityEngine;
using QuantumTek.QuantumInventory;

public class characterScript : MonoBehaviour
{
    ///Declaring shit
    [SerializeField] private float speed = 1;
    [SerializeField] private float stamina = 10;
    [SerializeField] private float staminacooldown = 1;
    [SerializeField] private InputActionProperty move;
    [SerializeField] private InputActionProperty sprint;
    [SerializeField] private InputActionProperty crouch;
    [SerializeField] private InputActionProperty interact;
    [SerializeField] private Canvas dialogCanvas;
    [SerializeField] private GameObject invent;
    [SerializeField] private QI_Chest chestScript;
    private bool sprintLF = false;
    private BasicInkExample BasicInkExample;
    private Animator anim;
    private Rigidbody2D rb;
    private Transform trans;
    
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        trans = GetComponent<Transform>();
        BasicInkExample = GetComponent<BasicInkExample>();
        BasicInkExample.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!(dialogCanvas.transform.childCount != 0 || invent.activeSelf))
        {

            trans.localScale = new Vector3(1, 1, 1);
            speed = 1;

            Vector2 movevector = move.action.ReadValue<Vector2>();

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

            if (interact.action.WasPressedThisFrame())
            {
                Transform cast = Physics2D.BoxCast(transform.position, new Vector2(0.5f, 0.5f),0, new Vector2(anim.GetFloat("x"), anim.GetFloat("y")),0.4f, 8).transform;
                if (cast != null)
                {
                    if (cast.GetComponent<simpleInkStorage>() != null)
                    {
                        BasicInkExample.inkJSONAsset = cast.GetComponent<simpleInkStorage>().inkStorage;
                        BasicInkExample.enabled = true;

                        if (cast.gameObject.tag == "Chest")
                        {
                            
                        }


                    }
                }
            }
        }
        else
        {
            BasicInkExample.enabled = false;
            rb.velocity = Vector3.zero;
            anim.SetBool("moving", false);
        }
    }
}
