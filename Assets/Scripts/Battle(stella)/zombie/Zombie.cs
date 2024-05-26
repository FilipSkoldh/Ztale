using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : MonoBehaviour
{
    [SerializeField] private GameObject bloodDrop;

    public void Cough()
    {
        for (int i = 0; i < 4; i++)
        {
            GameObject go = Instantiate(bloodDrop, new Vector3(0, 2, 0), Quaternion.identity);
            go.GetComponent<Rigidbody2D>().velocity = Random.insideUnitCircle * 2; 
        }
    }
}
