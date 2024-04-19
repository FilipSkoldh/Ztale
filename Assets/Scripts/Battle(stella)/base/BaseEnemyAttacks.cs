using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BaseEnemyAttacks : MonoBehaviour
{
    [SerializeField] private BulletHellController bulletHell;
    [SerializeField] private Box box;
    [SerializeField] private EventSystem eventSystem;
    [SerializeField] private GameObject attackButton;

    private float timer;
    
    public void Attack()
    {
        box.width = 2; 
        box.height = 2;
        bulletHell.enabled = true;
        bulletHell.transform.position = new Vector2(0, -1);
        timer = 5;
    }

    private void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;

        }
        else if (timer < 0)
        {
            timer = 0;
            bulletHell.Hide();
            bulletHell.enabled = false;
            eventSystem.SetSelectedGameObject(attackButton);
            box.width = 6;
            box.height = 2;
        }
    }
}
