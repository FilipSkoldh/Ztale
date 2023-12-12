using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class testEnemy : MonoBehaviour
{
    public enemyLife el;
    int damage = 0;
    public TextMeshProUGUI text;
    string responce = "";
    public InputActionAsset actions;
    private InputAction interact;
    bool interacting = true;
    public shooting shootin;
    public bullet_hell_controller hell;
    public Transform player;
    public GameObject bullet;
    int attack1 = 10;
    float time = 0.25f;
    private void OnEnable()
    {
        actions.FindActionMap("char").Enable();
    }

    private void Awake()
    {
        interact = actions.FindActionMap("char").FindAction("interact");
    }

    public void Start()
    {
        el = GetComponent<enemyLife>();

    }

    public void Update()
    {
        GameObject tempO = text.transform.parent.gameObject;

        interacting = interact.WasPressedThisFrame();
        if (el.playerAction != "")
        {
            Debug.Log("sees");
            if (el.playerAction.Contains("HIT"))
            {
                string temp = el.playerAction.Remove(0, 3);
                damage = int.Parse(temp);

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
            attack1 = 0;
            hell.enabled = true;
            player.position = new Vector3(0,-1,0);

        }

        if (attack1 < 10)
        {
            if (time < 0) 
            { 
                time = 0.25f;
                Instantiate(bullet, new Vector3(Random.Range(-1f, 1f), -0.1f, 0), new Quaternion(0, 0, 0, 0));
                attack1++;
            }
            else
            {
                time -= Time.deltaTime;
            }
        }
    }

}
