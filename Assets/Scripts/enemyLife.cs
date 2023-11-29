using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyLife : MonoBehaviour
{
    public int HP;
    public string scriptName;
    public Type type;

    public void Awake()
    {
        type = Type.GetType(scriptName);
    }
    public void hit(int damage)
    {
        HP -= damage;
        type.hit();
    }
}
