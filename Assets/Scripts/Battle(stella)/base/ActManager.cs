using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ActManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> buttons = new();
    [SerializeField] private EventSystem eventSystem;
    [SerializeField] private GameObject actButton;
    [SerializeField] private GameObject noButton;
    private BaseEnemyRelay selectedEnemy;

    [SerializeField] private InputActionProperty backInput;

    private bool selectingEnemy;
    private bool selectingAct;
    // Start is called before the first frame update
    void Start()
    {
        
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

            eventSystem.SetSelectedGameObject(noButton);
            for (int i = 0; i < buttons.Count; i++)
            {
                buttons[i].gameObject.SetActive(false);
            }
        }
    }
}
