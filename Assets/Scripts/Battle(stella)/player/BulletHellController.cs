using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class BulletHellController : MonoBehaviour
{
    public InputActionProperty move;
    private Rigidbody2D rb;
    private CircleCollider2D cc;
    private Transform t;
    public float speed;

    [SerializeField] private EventSystem eventSystem;
    [SerializeField] private GameObject shootbutton;

    private bool moveable = false;
    // Start is called before the first frame update
    private void OnEnable()
    {

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
        if (moveable)
            rb.velocity = movement * speed;
    }

    public void StopBulletHell()
    {
        rb.velocity = Vector2.zero;
        t.position = new Vector2(-10, 0);
        eventSystem.SetSelectedGameObject(shootbutton);
        moveable = false;
    }
    public void StartBulletHell(Vector2 position)
    {
        cc.radius = 0.14f;
        t.position = position;
        eventSystem.SetSelectedGameObject(null);
        moveable = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<BaseProjectile>() != null)
        {
            GlobalVariables.Hp -= collision.GetComponent<BaseProjectile>().damage;

            if (collision.GetComponent<BaseProjectile>().singleHit)
            {
                Destroy(collision.gameObject);
            }
        }
    }
}
