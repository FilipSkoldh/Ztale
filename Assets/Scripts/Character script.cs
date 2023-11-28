using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public class Characterscript : MonoBehaviour
{
    ///Declaring shit
    public float speed = 1;
    public float sprint = 0;
    public float stamina = ;
    public InputActionAsset actions;
    private InputAction moveaction;
    private InputAction sprintaction;
    private Rigidbody2D rb;

    void OnEnable()
    {
        actions.FindActionMap("char").Enable();
    }

    private void Awake()
    {
        moveaction = actions.FindActionMap("char").FindAction("move");
        sprintaction = actions.FindActionMap("char").FindAction("sprint");
        rb = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Vector2 movevector = moveaction.ReadValue<Vector2>();
        sprint = sprintaction.ReadValue<float>();
        if (sprint > 0)
        {
            speed = 2;
            stamina = stamina - Time.deltaTime * 1;
        }
        else
        {
            speed = 1;
        }
        Debug.Log(movevector);
        rb.velocity = movevector * speed;
    }
}
