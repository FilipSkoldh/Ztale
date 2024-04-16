using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class coughScript : MonoBehaviour
{
    public Rigidbody2D rb;
    float timer = 1;

    void Start()
    {
        rb.AddForce(new Vector3(Random.Range(-90, 90), Random.Range(-100, 0), 0));
    }


    private void Update()
    {
        timer -= Time.deltaTime;

        if (timer < 0)
        {
            Destroy(this.gameObject);

        }

        Vector2 v = rb.velocity;
        float angle = Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("hit");
        if (collision.gameObject.layer == 512)
        {
            collision.GetComponent<BulletHellController>().HP -= 1;
            Destroy(this.gameObject);
        }
    }
}
