using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class testEnemy : MonoBehaviour
{
    public enemyLife el;
    int damage = 0;
    public TextMeshProUGUI text;

    public void Start()
    {
        el = GetComponent<enemyLife>();
    }


    public void Update()
    {
        if (el.playerAction != "")
        {
            if (el.playerAction.Contains("HIT"))
            {
                string temp = el.playerAction.Remove(0, 3);
                damage = int.Parse(temp);
                Debug.Log(damage);

                el.playerAction = "";
            }
            GameObject tempO = text.transform.parent.gameObject;
            tempO.SetActive(true);

            
        }
    }
}
