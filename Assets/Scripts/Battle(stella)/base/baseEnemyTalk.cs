using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class BaseEnemyTalk : MonoBehaviour
{
    [SerializeField] private List<string> enemyLines = new List<string>();

    private BattleManager battleManager;
    private BaseEnemyAttacks attacks;
    private GameObject speechBubble;
    private InputActionProperty interactProperty;

    private void Start()
    {
        battleManager = transform.parent.GetComponent<BattleManager>();
        interactProperty = battleManager.interactProperty;
        attacks = battleManager.GetComponent<BaseEnemyAttacks>();
        speechBubble = transform.GetChild(0).GetChild(0).gameObject;
    }

    public void Talk(int line)
    {
        speechBubble.SetActive(true);
        speechBubble.GetComponentInChildren<TextMeshProUGUI>().text = enemyLines[line];
    }

    // Update is called once per frame
    void Update()
    {
        if (speechBubble.activeSelf && interactProperty.action.WasPressedThisFrame())
        {
            speechBubble.SetActive(false);
            attacks.Attack();
        }
    }
}
