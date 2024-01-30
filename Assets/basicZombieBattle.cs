using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class basicZombieBattle : MonoBehaviour
{
    public float agro;
    public GameObject slashPrefab;
    public Animator animator;
    public enemyLife relay;
    public bullet_hell_controller bulletHell;
    public box box;


    private GameObject slash0;
    private GameObject slash1;
    private GameObject slash2;

    private void Start()
    {
        animator.speed = agro*0.1f + 0.5f;
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
    void SlashTry()
    {
        if (Random.Range(0, 100) < agro * 5)
        {
            animator.SetTrigger("slash");
            Debug.Log("y");
        }
        else
        {
            Debug.Log("n");
        }
    }
    private void Update()
    {
        if(relay.playerAction != "")
        {
            bulletHell.enabled = true;
            if(Random.Range(0, 1) == 0)
            {
                animator.SetTrigger("slash");
                box.width = 2;
                box.height = 2;
            }
            else
            {
                animator.SetTrigger("cough");
                box.height = 2;
                box.width = 2;
            }
            relay.playerAction = "";
        }
    }
}
