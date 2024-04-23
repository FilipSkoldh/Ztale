using QuantumTek.QuantumInventory;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class ActNItemManager : MonoBehaviour
{
    private BattleManager battleManager;
    private InputActionProperty backProperty;
    private InputActionProperty interactProperty;
    private GameObject actButton;
    private GameObject itemButton;
    private List<GameObject> buttons = new();
    private EventSystem eventSystem;
    private QI_Inventory inventory;
    private TextMeshProUGUI actingText;
    private BaseEnemyAttacks attacks;

    private BaseEnemyRelay selectedEnemy;
    private bool selectingEnemy;
    private bool selectingAct;
    private bool selectingItem;
    private bool usingItem;
    // Start is called before the first frame update
    void Start()
    {
        battleManager = GetComponent<BattleManager>();
        backProperty = battleManager.backProperty;
        interactProperty = battleManager.interactProperty;
        actButton = battleManager.buttons[1];
        itemButton = battleManager.buttons[2];
        buttons = battleManager.buttons;
        buttons.RemoveRange(0, 3);
        eventSystem = battleManager.eventSystem;
        inventory = battleManager.inventory;
        actingText = battleManager.actingText;
        attacks = GetComponent<BaseEnemyAttacks>();
    }

    // Update is called once per frame
    void Update()
    {
        if (selectingEnemy && backProperty.action.WasPressedThisFrame())
        {
            eventSystem.SetSelectedGameObject(actButton);
            for (int i = 0; i < buttons.Count; i++)
            {
                buttons[i].gameObject.SetActive(false);

            }
            selectingEnemy = false;
        }
        if (selectingItem && backProperty.action.WasPressedThisFrame())
        {
            eventSystem.SetSelectedGameObject(itemButton);
            for (int i = 0; i < buttons.Count; i++)
            {
                buttons[i].gameObject.SetActive(false);

            }
            selectingItem = false;
        }
        if (selectingAct && backProperty.action.WasPressedThisFrame())
        {
            selectingAct = false;
            EnemySelect();
        }
        if(usingItem && interactProperty.action.WasPressedThisFrame())
        {
            actingText.text = "";
            attacks.Attack();
        }
        usingItem = actingText.text != "";
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
        if (inventory.Stacks.Count != 0)
        {
            selectingItem = true;

            int numStacks = GlobalVariables.PlayerInventory.Count;
            for (int i = 0; i < buttons.Count; i++)
            {
                if (i < numStacks)
                {
                    buttons[i].SetActive(true);
                    if (inventory.Stacks[i].Amount > 1)
                        buttons[i].GetComponentInChildren<TextMeshProUGUI>().text = $"{inventory.Stacks[i].Item.name} x{inventory.Stacks[i].Amount}";
                    else
                        buttons[i].GetComponentInChildren<TextMeshProUGUI>().text = $"{inventory.Stacks[i].Item.name}";
                }
                else
                {
                    buttons[i].SetActive(false);
                }
            }
            eventSystem.SetSelectedGameObject(buttons[0]);
        }
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
                buttons[i].SetActive(false);
            }
            selectingAct = false;
        }
        else if (selectingItem)
        {
            QI_ItemData item = inventory.Stacks[whichButton].Item;
            inventory.RemoveItem(item.Name, 1);

            if (item.GetType().ToString() == "QI_Healing")
            {
                GlobalVariables.Hp = Mathf.Clamp(GlobalVariables.Hp + (item as QI_Healing).healingAmount,0,GlobalVariables.MaxHp);
            }
            else if (item.GetType().ToString() == "QI_Weapons")
            {
                if (GlobalVariables.EquippedWeapon != null)
                {
                    inventory.AddItem(GlobalVariables.EquippedWeapon, 1);
                }

                GlobalVariables.EquippedWeapon = (item as QI_Weapons);
            }
            else if (item.GetType().ToString() == "QI_Equipment")
            {
                if (GlobalVariables.EquippedEquipment != null)
                {
                    inventory.AddItem(GlobalVariables.EquippedEquipment, 1);
                }
                GlobalVariables.EquippedEquipment = (item as QI_Equipment);
            }
            actingText.text = item.Description;
            for (int i = 0; i < buttons.Count; i++)
            {
                buttons[i].SetActive(false);
            }
        }
    }
}
