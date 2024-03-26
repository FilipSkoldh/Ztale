using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class testEnemy : MonoBehaviour
{
    int damage = 0;
    public TextMeshProUGUI text;
    string responce = "";
    public InputActionProperty interact;
    bool interacting = true;
    public shooting shootin;
    public bullet_hell_controller hell;
    public Transform player;
    public GameObject bullet;
    public box box;


    int attack1 = 10;
    bool battack1 = false;
    float time = 0.25f;


    public void Start()
    {
        el = GetComponent<enemyLife>();

    }

    public void Update()
    {
        GameObject tempO = text.transform.parent.gameObject;

        interacting = interact.action.WasPressedThisFrame();
        if (el.playerAction != "")
        {
            Debug.Log("sees");
            if (el.playerAction == "HIT")
            {
                responce = "HA! That didn't even tickle!";
            }
            else if (el.playerAction.Contains("MISS"))
            {
                responce = "HA! You can't even hit me";
            }
            el.playerAction = "";
            shootin.enabled = false;

            tempO.SetActive(true);
            text.text = responce;


        }



        if (interacting && tempO.activeSelf)
        { 
            tempO.SetActive(false);
            box.height = 2;
            box.width = 2;
            attack1 = 0;
            hell.enabled = true;
            player.position = new Vector3(0,-1,0);

        }

        if (battack1)
        {
            if (attack1 < 10)
            {
                if (time < 0)
                {
                    time = 0.25f;
                    Instantiate(bullet, new Vector3(Random.Range(-1f, 1f), -0.1f, 0), new Quaternion(0, 0, 0, 0));
                    attack1++;
                    if (attack1 > 9)
                    {
                        time = 2;
                    }
                }
                else
                {
                    time -= Time.deltaTime;
                }
            }
            else
            {

            }
        }
    }

}
