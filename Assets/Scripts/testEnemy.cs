using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testEnemy : MonoBehaviour
{
    public enemyLife el;
    int damage = 0;

    public void Start()
    {
        el = GetComponent<enemyLife>();
    }


    public void Update()
    {
        if (el.playerAction.Contains("HIT"))
        {
            string temp = el.playerAction.Remove(0, 3);
            damage.Parse(temp);
        }
    }
}
