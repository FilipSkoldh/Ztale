using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using TreeEditor;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class BaseEnemyRelay : MonoBehaviour
{
    private BattleManager battleManager;
    private InputActionProperty interactProperty;
    private BaseEnemyTalk talk;
    private TextMeshProUGUI healthText;

    public int hp;
    [SerializeField] private int maxhp;
    public List<string> acts = new();
    public List<string> actDescriptions = new();
    public List<int> spareActs = new();
    private TextMeshProUGUI actingText;



    private int act;
    private bool acting;
    private bool acter;

    private void Awake()
    {
        battleManager = transform.parent.GetComponent<BattleManager>();
        interactProperty = battleManager.interactProperty;
        actingText = battleManager.actingText;
        talk = GetComponent<BaseEnemyTalk>();
        healthText = transform.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>();
        healthText.text = $"{hp} / {maxhp}";
    }
    public void Hit(int damage)
    {
        hp -= damage;
        healthText.text = $"{hp} / {maxhp}";
        if (hp <= 0)
        {
            transform.parent.GetComponent<BattleManager>().enemyStates[transform.GetSiblingIndex()] = 1;

        }
        else
        {
            if (spareActs.Count > 0)
            {
                if (spareActs[spareActs.Count - 1] == 0)
                {
                    spareActs.RemoveAt(spareActs.Count - 1);
                    talk.Talk(2 + acts.Count + spareActs.Count);
                }
                else
                {
                    talk.Talk(0);
                }
            }
            else
            {
                talk.Talk(0);
            }
        }
    }
    public void Miss()
    {
        if (spareActs.Count > 0)
        {
            if (spareActs[spareActs.Count - 1] == 1)
            {
                spareActs.RemoveAt(spareActs.Count - 1);
                talk.Talk(2 + acts.Count + spareActs.Count);
            }
            else
            {
                talk.Talk(1);
            }
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
            transform.parent.GetComponent<BattleManager>().enemyStates[transform.GetSiblingIndex()] = 2;

        }
        else
        {
            if (spareActs.Count != 0)
            {
                if (spareActs[spareActs.Count - 1] == action + 2)
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
            }
            else
            {
                act = action;
                actingText.text = actDescriptions[action];
            }
            acting = true;
        }
    }
    private void Update()
    {
        if (acter && interactProperty.action.WasPressedThisFrame())
        {
            acting = false;
            actingText.text = "";
            talk.Talk(act + 2);
        }
        acter = acting;
    }
}
