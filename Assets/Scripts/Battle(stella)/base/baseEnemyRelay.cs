using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class BaseEnemyRelay : MonoBehaviour
{
    public int HP;
    public List<string> acts = new();
    public List<string> actDescriptions = new();
    public List<int> spareActs = new();
    public BaseEnemyTalk talk;
    [SerializeField] private TextMeshProUGUI actingText;

    [SerializeField] private InputActionProperty interactAction;


    private int act;
    private bool acting;
    private bool acter;
    private bool interactPressedLastFrame;
    public void Hit(int damage)
    {
        HP -= damage;
        if (HP <= 0)
        {
            Debug.Log("killed");
            gameObject.SetActive(false);
        }

        if (spareActs[spareActs.Count - 1] == 0 && spareActs.Count > 0)
        {
            spareActs.RemoveAt(spareActs.Count - 1);
            talk.Talk(2 + acts.Count + spareActs.Count);
        }
        else
        {
            talk.Talk(0);
        }
    }
    public void Miss()
    {
        if (spareActs[spareActs.Count - 1] == 1 && spareActs.Count > 0)
        {
            spareActs.RemoveAt(spareActs.Count - 1);
            talk.Talk(2 + acts.Count + spareActs.Count);
        }
        else
        {
            talk.Talk(1);
        }
    }
    public void Act(int action)
    {
        if (action == acts.Count)
        {
            Debug.Log("spared");
            gameObject.SetActive(false);
        }

        if (spareActs[spareActs.Count - 1] == action + 2 && spareActs.Count > 0)
        {
            spareActs.RemoveAt(spareActs.Count - 1);
            act = acts.Count + spareActs.Count;
            actingText.text = actDescriptions[acts.Count + spareActs.Count];
        }
        else
        {
            act = action;
            actingText.text = actDescriptions[action];
        }
        acting = true;
    }
    private void Update()
    {
        if (acter && interactAction.action.WasPressedThisFrame())
        {
            acting = false;
            actingText.text = "";
            talk.Talk(act + 2);
        }
        acter = acting;
    }
}
