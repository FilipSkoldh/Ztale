using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Escapemenu : MonoBehaviour
{
    [SerializeField] private InputActionProperty esc;
    [SerializeField] private GameObject escMenu;
    [SerializeField] private EventSystem EventSystem;

    void Update()
    {
        if (esc.action.WasPressedThisFrame())
        {
            if (!escMenu.activeSelf)
            {

                escMenu.SetActive(true);
                EventSystem.SetSelectedGameObject(escMenu.transform.GetChild(1).GetChild(0).gameObject);
                Debug.Log("esc pressed");
            }
            else
            {
                escMenu.SetActive(false);
            }
        }
    }
    public void ExitGame()
    {
        Application.Quit();
    }
    public void MainMenu()
    {
        SceneManager.LoadScene("Main menu");
    }
}
