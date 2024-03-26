using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemyAttacks : MonoBehaviour
{
    [SerializeField] private BulletHellController bulletHell;
    [SerializeField] private Box box;
    private float timer;
    
    public void Attack()
    {
        box.width = 2; 
        box.height = 2;
        bulletHell.enabled = true;
        timer = 5;
    }

    private void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;

        }
        else
        {
            timer = 0;
            bulletHell.enabled = false;
            box.width = 6;
            box.height = 2;
        }
    }
}
