using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class BaseEnemyTalk : MonoBehaviour
{
    [SerializeField] private List<string> enemyLines = new List<string>();

    private BattleManager battleManager;
    private GameObject speechBubble;
    private InputActionProperty interactProperty;
    private bool talking;

    private void Start()
    {
        battleManager = transform.parent.GetComponent<BattleManager>();
        interactProperty = battleManager.interactProperty;
        speechBubble = transform.GetChild(0).GetChild(0).gameObject;
    }

    public void Talk(int line)
    {
        Debug.Log("talk");
        speechBubble.SetActive(true);
        speechBubble.GetComponentInChildren<TextMeshProUGUI>().text = enemyLines[line];
    }

    // Update is called once per frame
    void Update()
    {
        if (talking && interactProperty.action.WasPressedThisFrame())
        {
            speechBubble.SetActive(false);
            battleManager.StartAttack();
        }
        talking = speechBubble.activeSelf;
    }
}
