using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class shooting : MonoBehaviour
{
    public InputActionProperty move;
    public InputActionProperty shoot;

    public Transform enemies;

    private Rigidbody2D rb;
    private CircleCollider2D cc;
    private Transform t;

    public float speed;

    private RaycastHit2D[] hits;
    private Transform hitT;

    // Start is called before the first frame update
    void Start()
    {
        cc.radius = 0.03f;
        t.position = new Vector3(0f, 1f);

    }



    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        cc = GetComponent<CircleCollider2D>();
        t = GetComponent<Transform>();

    }

    // Update is called once per frame
    void Update()
    {
        Vector2 movement = move.action.ReadValue<Vector2>();
        bool shot = shoot.action.WasPressedThisFrame();
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
                hitT.GetComponent<BaseEnemyRelay>().Hit(1);

                for (int i = 0; i < enemies.childCount; i++)
                {
                    if (enemies.GetChild(i) != hitT)
                    {
                        enemies.GetChild(i).GetComponent<BaseEnemyRelay>().Miss();
                    }
                }

            }
            else
            {
                for (int i = 0; i < enemies.childCount; i++)
                {
                    enemies.GetChild(i).GetComponent<BaseEnemyRelay>().Miss();
                }
            }
            this.enabled = false;
        }
    }
}
