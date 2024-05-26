using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BaseEnemyAttacks : MonoBehaviour
{
    protected BattleManager battleManager;
    protected BulletHellController bulletHell;
    protected Box box;

    protected float timer;

    private void Awake()
    {
        battleManager = GetComponent<BattleManager>();
        bulletHell = battleManager.bulletHell;
        box = battleManager.box;
    }

    public virtual void Attack()
    {
        box.width = 2; 
        box.height = 2;
        bulletHell.StartBulletHell(new Vector2(0, -1));
        timer = 1;
    }

    private void Update()
    {
        Timer(Time.deltaTime);
    }

    protected void Timer(float deltaTime)
    {
        if (timer > 0)
        {
            timer -= deltaTime;
        }
        else if (timer < 0)
        {
            timer = 0;
            EndAttack();
            box.width = 6;
            box.height = 2;
        }
    }

    public virtual void EndAttack()
    {
        battleManager.EndAttack();
    }
}
