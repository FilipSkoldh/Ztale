using QuantumTek.QuantumInventory;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class BattleManager : MonoBehaviour
{
    public List<int> enemyStates = new();

    public InputActionProperty interactProperty;
    public InputActionProperty backProperty;
    public TextMeshProUGUI actingText;
    public List<GameObject> buttons;
    public EventSystem eventSystem;
    public BulletHellController bulletHell;
    public Box box;
    public QI_Inventory inventory;

    private bool winning = false;

    private void Awake()
    {
        inventory.Stacks = GlobalVariables.PlayerInventory;
    }

    // Update is called once per frame
    void Update()
    {
        if (winning && interactProperty.action.WasPressedThisFrame())
        {
            SceneManager.LoadScene("Overworld");
        }
        if (EnemiesDefeated())
        {
            actingText.text = $"You win!";
            winning = true;
        }
    }

    public void SelectNothing()
    {
        eventSystem.SetSelectedGameObject(null);
    }
    private bool EnemiesDefeated()
    {
        foreach (int state in enemyStates)
        {
            if (state == 0)
            {
                return false;
            }
        }
        return true;
    }
}
