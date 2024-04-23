using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BaseEnemyAttacks : MonoBehaviour
{
    private BattleManager battleManager;
    private BulletHellController bulletHell;
    private Box box;

    private float timer;

    private void Awake()
    {
        battleManager = GetComponent<BattleManager>();
        bulletHell = battleManager.bulletHell;
        box = battleManager.box;
    }

    public void Attack()
    {
        box.width = 2; 
        box.height = 2;
        bulletHell.StartBulletHell(new Vector2(0, -1));
        timer = 5;
    }

    private void Update()
    {
        Timer(Time.deltaTime);
    }

    private void Timer(float deltaTime)
    {
        if (timer > 0)
        {
            timer -= deltaTime;
        }
        else if (timer < 0)
        {
            timer = 0;
            battleManager.EndAttack();
            box.width = 6;
            box.height = 2;
        }
    }
}
