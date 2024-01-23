using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyLife : MonoBehaviour
{
    public int HP;
    [NonSerialized]public String playerAction = "";

    public void hit(int damage)
    {
        HP -= damage;
        playerAction = "HIT";
        Debug.Log(playerAction);
    }
    public void MISS()
    {
        playerAction = "MISS";
    }
}
