using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

/// <summary>
/// the attack script for a single zombie
/// </summary>
public class SingleZombieAttacks : BaseEnemyAttacks
{

    override public void Attack()
    {
        box.width = 2;
        box.height = 2;
        bulletHell.StartBulletHell(new Vector2(0, -1));
        timer = 3;
        transform.GetChild(0).GetComponent<Animator>().SetBool("cough", true);
    }

    override public void EndAttack()
    {
        transform.GetComponentInChildren<Animator>().SetBool("cough", false);
        battleManager.EndAttack();
    }
}
