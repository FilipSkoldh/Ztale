using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class coughScript : MonoBehaviour
{
    public Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        rb.AddForce(new Vector3 (60,0,0));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
