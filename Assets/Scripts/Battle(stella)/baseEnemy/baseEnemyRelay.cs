using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemyRelay : MonoBehaviour
{
    public int HP;
    public List<string> acts = new();
    public List<string> spareActs = new();
    public BaseEnemyTalk talk;


    public void Hit(int damage)
    {
        HP -= damage;
        talk.Talk(0);
    }
    public void Miss()
    {
        talk.Talk(1);
    }
    public void Act(string action)
    {
        talk.Talk(acts.IndexOf(action) + 2);
    }
}
