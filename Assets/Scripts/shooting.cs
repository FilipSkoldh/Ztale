using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    private RaycastHit2D[] hits;
    private SpriteRenderer sr;
    private Transform hitT;

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
        rb.velocity = movement * speed;

        if (shot)
        {
            Debug.Log("shot");
            hits = Physics2D.CircleCastAll(t.position, 0.02f, Vector2.zero, 0, 256);
            int sorrtingOrder = -10;

            foreach (RaycastHit2D i in hits)
            {
                if (i.transform != null && i.transform.gameObject.GetComponent<SpriteRenderer>().sortingOrder > sorrtingOrder)
                {
                    sorrtingOrder = i.transform.gameObject.GetComponent<SpriteRenderer>().sortingOrder;
                    hitT = i.transform;

                }
            }
            if (hitT != null)
            {
                hitT.GetComponent<enemyLife>().hit(1);

            }

        }
    }
}
