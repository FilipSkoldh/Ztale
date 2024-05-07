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
    public List<bool> enemyAttacking = new();

    public InputActionProperty interactProperty;
    public InputActionProperty backProperty;
    public TextMeshProUGUI actingText;
    public GameObject shootButton;
    public GameObject actButton;
    public GameObject itemButton;
    public GameObject[] buttons;
    public EventSystem eventSystem;
    public BulletHellController bulletHell;
    public Box box;
    public QI_Inventory inventory;

    public BaseEnemyAttacks enemyAttacks;

    public int useTime;

    private bool winning = false;

    private void Awake()
    {
        inventory.Stacks = GlobalVariables.PlayerInventory;

        enemyAttacks = gameObject.AddComponent<BaseEnemyAttacks>();

        for (int i = 0; i < transform.childCount; i++)
        {
            enemyStates.Add(0);
            enemyAttacking.Add(false);
        }
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
            actingText.text = "You win!";
            winning = true;
        }
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

    public void EndAttack()
    {
        bulletHell.StopBulletHell();
        eventSystem.SetSelectedGameObject(shootButton);
    }

    public void StartAttack(int siblingIndex)
    {
        enemyAttacking[siblingIndex] = true;
        bool attack = true;
        foreach(bool bol in enemyAttacking)
        {
            if (!bol)
            {
                attack = false;
            }
        }

        if (attack)
        {
            enemyAttacks.Attack();
        }
    }

    public void StopAnimation()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).GetComponent<Animator>().speed = 0;
        }
    }

    public void StartAnimation()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).GetComponent<Animator>().speed = 1;
        }
    }
}
