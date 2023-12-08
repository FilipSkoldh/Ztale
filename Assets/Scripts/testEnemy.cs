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
        interacting = interact.WasPressedThisFrame();
        if (el.playerAction != "")
        {
            Debug.Log("sees");
            if (el.playerAction.Contains("HIT"))
            {
                string temp = el.playerAction.Remove(0, 3);
                damage = int.Parse(temp);
                Debug.Log(damage);
                responce = "HA! That didn't even tickle!";
            }
            else if (el.playerAction.Contains("MISS"))
            {
                responce = "HA! You can't even hit me";
            }
            el.playerAction = "";
            shootin.enabled = false;
            GameObject tempO = text.transform.parent.gameObject;
            tempO.SetActive(true);
            text.text = responce;
            StartCoroutine(Cupdate());
            Debug.Log("works");


        }
    }
    IEnumerator Cupdate()
    {

        yield return new WaitUntil(() => interacting);
    }
}
