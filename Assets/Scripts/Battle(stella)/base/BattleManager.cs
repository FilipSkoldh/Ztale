using QuantumTek.QuantumInventory;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

/// <summary>
/// a script that manages most parts of the battle system
/// </summary>
public class BattleManager : MonoBehaviour
{
    // states of the diffrent enemies 0 is fighting 1 is dead 2 is spared
    public List<int> enemyStates = new();
    public List<bool> enemyAttacking = new();

    //controls
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

    public GameObject zombie;

    private void Awake()
    {
        
        switch (GlobalVariables.Encounter)
        {
            case 0:
                GameObject temp = Instantiate(zombie, transform);
                temp.transform.position = new Vector3(0,1.5f,0);
                temp.name = "Zombie";
                enemyAttacks = gameObject.AddComponent<SingleZombieAttacks>();
                break;
        }

        inventory.Stacks = GlobalVariables.Inventories[0];


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
            switch(GlobalVariables.Encounter)
            {
                case 0:
                    GlobalVariables.ZombieDead = true;
                    break;
            }

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
        for(int i = 0; i < transform.childCount;i++)
        {
            if (!enemyAttacking[i] && enemyStates[i] == 0)
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
