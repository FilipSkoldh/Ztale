using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class basicZombieBattle : MonoBehaviour
{
    public float agro;
    public GameObject slashPrefab;
    public Animator animator;


    private GameObject slash0;
    private GameObject slash1;
    private GameObject slash2;

    private void Start()
    {
        animator.speed = 1+(agro-1)*0.1f;
    }
    void SlashSpawn()
    {
        slash0 = Instantiate(slashPrefab, new Vector3(Random.Range(-0.9f, 0.9f), 0.1f), Quaternion.identity);
        slash1 = Instantiate(slashPrefab, new Vector3(Random.Range(-0.9f, 0.9f), 0.1f), Quaternion.identity);
        slash2 = Instantiate(slashPrefab, new Vector3(Random.Range(-0.9f, 0.9f), 0.1f), Quaternion.identity);
    }
    void SlashActivate()
    {
        slash0.GetComponent<slash>().activate();
        slash1.GetComponent<slash>().activate();
        slash2.GetComponent<slash>().activate();
    }
}
