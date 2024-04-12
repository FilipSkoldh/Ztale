using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class BattleManager : MonoBehaviour
{
    public List<int> enemyStates = new();

    public InputActionProperty interactProperty;
    public InputActionProperty backProperty;

    private TextMeshProUGUI actingText;
    private bool winning = false;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (winning && interactProperty.action.WasPressedThisFrame())
        {
            Debug.Log("load");
            SceneManager.LoadScene("Scene 1");
        }
        if (EnemiesDefeated())
        {
            actingText.text = $"You win!";
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
}
