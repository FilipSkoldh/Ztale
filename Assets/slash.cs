using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class slash : MonoBehaviour
{
    public Collider2D collider;
    float timer = 0.5f;
    bool activated = false;
    private void Update()
    {
        if (activated)
        {
            timer -= Time.deltaTime;
            Debug.Log("1");
        }

        if (timer < 0)
        {
            Destroy(this.gameObject);
            Debug.Log("2");
        }
    }
    public void activate()
    {
        transform.localScale = new Vector3(0.1f, 2.2f, 1);
        transform.position = new Vector3(transform.position.x, -1, 0);
        activated = true;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 512)
        {
            collision.GetComponent<bullet_hell_controller>().HP -= 1;
            Destroy(this.gameObject);
        }
    }
}
