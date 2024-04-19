using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ActNItemManager : MonoBehaviour
{
    private BattleManager battleManager;
    private InputActionProperty backInput;
    private GameObject actButton;
    private GameObject itemButton;
    private List<GameObject> buttons = new();
    private EventSystem eventSystem;

    private BaseEnemyRelay selectedEnemy;
    private bool selectingEnemy;
    private bool selectingAct;
    private bool selectingItem;
    // Start is called before the first frame update
    void Start()
    {
        battleManager = GetComponent<BattleManager>();
        backInput = battleManager.backProperty;
        actButton = battleManager.buttons[1];
        itemButton = battleManager.buttons[2];
        buttons = battleManager.buttons;
        buttons.RemoveRange(0, 3);
        eventSystem = battleManager.eventSystem;
    }

    // Update is called once per frame
    void Update()
    {
        if (selectingEnemy && backInput.action.WasPressedThisFrame())
        {
            eventSystem.SetSelectedGameObject(actButton);
            for (int i = 0; i < buttons.Count; i++)
            {
                buttons[i].gameObject.SetActive(false);

            }
            selectingEnemy = false;
        }
        if (selectingItem && backInput.action.WasPressedThisFrame())
        {
            eventSystem.SetSelectedGameObject(itemButton);
            for (int i = 0; i < buttons.Count; i++)
            {
                buttons[i].gameObject.SetActive(false);

            }
            selectingItem = false;
        }

        if (selectingAct && backInput.action.WasPressedThisFrame())
        {
            selectingAct = false;
            EnemySelect();
        }
    }

    public void EnemySelect()
    {
        selectingEnemy = true;
        int numEnemies = transform.childCount;
        for (int i = 0; i < buttons.Count; i++)
        {
            if(i < numEnemies)
            {
                buttons[i].gameObject.SetActive(true);
                buttons[i].GetComponentInChildren<TextMeshProUGUI>().text = transform.GetChild(i).name;
            }
            else
            {
                buttons[i].gameObject.SetActive(false);
            }
        }
        eventSystem.SetSelectedGameObject(buttons[0]);
    }
    public void ItemSelect()
    {
        selectingItem = true;

        int numStacks = GlobalVariables.PlayerInventory.Count;
        for(int i = 0;i < buttons.Count; i++)
        {
            if (i < numStacks)
            {
                buttons[i].gameObject.SetActive(true);
                if (GlobalVariables.PlayerInventory[i].Amount > 1)
                    buttons[i].GetComponentInChildren<TextMeshProUGUI>().text = $"{GlobalVariables.PlayerInventory[i].Item.name} x{GlobalVariables.PlayerInventory[i].Amount}";
                else
                    buttons[i].GetComponentInChildren<TextMeshProUGUI>().text = $"{GlobalVariables.PlayerInventory[i].Item.name}";
            }
            else
            {
                buttons[i].gameObject.SetActive(false);
            }
        }
        eventSystem.SetSelectedGameObject(buttons[0]);
    }

    public void ButtonPress(int whichButton)
    {
        if (selectingEnemy)
        {
            selectingAct = true;
            selectingEnemy = false;
            selectedEnemy = transform.GetChild(whichButton).GetComponent<BaseEnemyRelay>();
            int numActs = selectedEnemy.acts.Count;
            for (int i = 0; i < buttons.Count; i++)
            {
                if (i < numActs)
                {
                    buttons[i].gameObject.SetActive(true);
                    buttons[i].GetComponentInChildren<TextMeshProUGUI>().text = selectedEnemy.acts[i];
                }
                else
                {
                    buttons[i].gameObject.SetActive(false);
                }
            }
            if (selectedEnemy.spareActs.Count == 0)
            {
                buttons[numActs].gameObject.SetActive(true);
                buttons[numActs].GetComponentInChildren<TextMeshProUGUI>().text = "Spare";
            }
            eventSystem.SetSelectedGameObject(buttons[0]);
        }
        else if (selectingAct)
        {
            selectedEnemy.Act(whichButton);

            eventSystem.SetSelectedGameObject(null);
            for (int i = 0; i < buttons.Count; i++)
            {
                buttons[i].gameObject.SetActive(false);
            }
            selectingAct = false;
        }
    }
}
