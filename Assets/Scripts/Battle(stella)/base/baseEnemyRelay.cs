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

    [SerializeField] private InputActionProperty interactAction;

    [SerializeField] private TextMeshProUGUI actingText;

    private int act;
    private bool acting;
    private bool acter;
    private bool interactPressedLastFrame;
    public void Hit(int damage)
    {
        HP -= damage;
        if (spareActs[spareActs.Count] == 0)
        {
            spareActs.RemoveAt(spareActs.Count);
        }
        talk.Talk(0);
    }
    public void Miss()
    {
        if (spareActs[spareActs.Count] == 1)
        {
            spareActs.RemoveAt(spareActs.Count);
        }
        talk.Talk(1);
    }
    public void Act(int action)
    {
        if (spareActs[spareActs.Count] == action + 2)
        {
            spareActs.RemoveAt(spareActs.Count);
        }
        act = action;
        actingText.text = actDescriptions[action];
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
