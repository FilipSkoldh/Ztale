using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class baseEnemyRelay : MonoBehaviour
{
    public int HP;
    public List<string> acts = new List<string>();
    public List<string> spareActs = new List<string>();


    public void Hit(int damage)
    {
        HP -= damage;
    }
    public void Miss()
    {

    }
    public void Act(string action)
    {

    }
}
