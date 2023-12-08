using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyLife : MonoBehaviour
{
    public int HP;
    public String playerAction = "";

    public void hit(int damage)
    {
        playerAction = "HIT" + damage;
    }
    public void MISS()
    {
        playerAction = "MISS";
    }
}
