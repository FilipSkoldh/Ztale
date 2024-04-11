using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class BaseEnemyTalk : MonoBehaviour
{ 
    [SerializeField] private List<string> enemyLines = new List<string>();
    [SerializeField] private GameObject speechBubble;
    [SerializeField] private BaseEnemyAttacks attacks;

    [SerializeField] private InputActionProperty interactAction;


    public void Talk(int line)
    {
        speechBubble.SetActive(true);
        speechBubble.GetComponentInChildren<TextMeshProUGUI>().text = enemyLines[line];
    }

    // Update is called once per frame
    void Update()
    {
        if (speechBubble.activeSelf && interactAction.action.WasPressedThisFrame())
        {
            speechBubble.SetActive(false);
            attacks.Attack();
        }
    }
}
