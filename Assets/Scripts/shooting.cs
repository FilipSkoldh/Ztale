using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class shooting : MonoBehaviour
{
    public InputActionAsset actions;
    private InputAction move;
    private InputAction shoot;
    private Rigidbody2D rb;
    private CircleCollider2D cc;
    private Transform t;
    public float speed;

    // Start is called before the first frame update
    void Start()
    {
        cc.radius = 0.03f;
        t.position = new Vector3(0f, 1f);

    }

    private void OnEnable()
    {
        actions.FindActionMap("char").Enable();
    }


    private void Awake()
    {
        move = actions.FindActionMap("char").FindAction("move");
        shoot = actions.FindActionMap("char").FindAction("interact");
        rb = GetComponent<Rigidbody2D>();
        cc = GetComponent<CircleCollider2D>();
        t = GetComponent<Transform>();

    }

    // Update is called once per frame
    void Update()
    {
        Vector2 movement = move.ReadValue<Vector2>();
        bool shot = shoot.WasPressedThisFrame();
        Debug.Log(movement);
        rb.velocity = movement * speed;

        if (shot)
        {

        }
    }
}
