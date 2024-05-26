using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class BulletHellController : MonoBehaviour
{
    //the move controls
    public InputActionProperty moveProperty;
    public InputActionProperty interactProperty;


    private Rigidbody2D rb;
    private CircleCollider2D cc;
    private Transform t;

    //the speed which the player moves
    public float speed;

    [SerializeField] private EventSystem eventSystem;
    [SerializeField] private GameObject shootbutton;
    [SerializeField] private Transform camera;
    [SerializeField] private GameObject enemies;
    [SerializeField] private GameObject canvas;

    //if the player is allowed to move
    private bool moveable = false;
    
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        cc = GetComponent<CircleCollider2D>();
        t = GetComponent<Transform>();  
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 movement = moveProperty.action.ReadValue<Vector2>();
        if (moveable)
            rb.velocity = movement * speed;

        if (GlobalVariables.Playerdead && interactProperty.action.WasPressedThisFrame())
        {
            SceneManager.LoadScene("Overworld");
        }
    }

    /// <summary>
    /// stops the bullethell
    /// </summary>
    public void StopBulletHell()
    {
        rb.velocity = Vector2.zero;
        t.position = new Vector2(-10, 0);
        moveable = false;
    }

    /// <summary>
    /// start the bullethell
    /// </summary>
    /// <param name="position">where the player starts</param>
    public void StartBulletHell(Vector2 position)
    {
        cc.radius = 0.14f;
        t.position = position;
        eventSystem.SetSelectedGameObject(null);
        moveable = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //is it a projectile?
        if (collision.GetComponent<BaseProjectile>() != null)
        {
            GlobalVariables.Hp -= collision.GetComponent<BaseProjectile>().damage;

            if (collision.GetComponent<BaseProjectile>().singleHit)
            {
                Destroy(collision.gameObject);
            }
        }

        //does the player die?
        if(GlobalVariables.Hp <= 0)
        {
            GetComponent<Animator>().SetTrigger("dead");
            camera.position += new Vector3(-10, 0, 0);
            transform.position += new Vector3(-10, 0, 0);
            enemies.SetActive(false);
            canvas.SetActive(false);
            moveable = false;
            rb.velocity = Vector2.zero;
            GlobalVariables.Playerdead = true;
        }
    }
}
