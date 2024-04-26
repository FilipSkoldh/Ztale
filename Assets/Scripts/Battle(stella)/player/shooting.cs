using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Shooting : MonoBehaviour
{
    public InputActionProperty move;
    public InputActionProperty shoot;
    public InputActionProperty back;

    public Transform enemies;
    public EventSystem eventSystem;
    public GameObject shootButton;

    private Rigidbody2D rb;
    private CircleCollider2D cc;
    private Transform t;

    public float speed;

    private RaycastHit2D[] hits;
    private Transform hitT;
    private bool shooting = false;

    [SerializeField] private List<Transform> bullets = new();

    public void StartShooting()
    {
        if (GlobalVariables.EquippedWeapon != null)
        {
            t.position = new Vector3(0f, 1f, 0f);
            cc.radius = 0.03f;
            shooting = true;
        }
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
        if (shooting)
        {
            Vector2 movement = move.action.ReadValue<Vector2>();
            rb.velocity = movement * speed;

            if (back.action.WasPressedThisFrame())
            {
                rb.velocity = Vector2.zero;
                transform.position = new Vector3(0, 10, 0);
                shooting = false;
                eventSystem.SetSelectedGameObject(shootButton);
            }

            if (shoot.action.WasPressedThisFrame() && eventSystem.currentSelectedGameObject == null)
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
                    hitT.GetComponent<BaseEnemyRelay>().Hit(GlobalVariables.EquippedWeapon.weaponDamage);
                }
                else
                {

                }
                rb.velocity = Vector2.zero;
                transform.position = new Vector3(0, 10, 0);
                shooting = false;
            }
            eventSystem.SetSelectedGameObject(null);
        }
    }
}
