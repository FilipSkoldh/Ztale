using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Escapemenu : MonoBehaviour
{
    //Declaring variables

    //All inputs needed in the script
    [SerializeField] private InputActionProperty esc;

    //the escMenu UI
    [SerializeField] private GameObject escMenu;

    //the scenes eventsystem
    [SerializeField] private EventSystem eventSystem;

    void Update()
    {
        //if ESC was pressed, toggle the esc menu UI
        if (esc.action.WasPressedThisFrame())
        {
            if (!escMenu.activeSelf)
            {
                //enables the esc menu and selects the first button
                escMenu.SetActive(true);
                eventSystem.SetSelectedGameObject(escMenu.transform.GetChild(1).GetChild(0).gameObject);
            }
            else
            {
                escMenu.SetActive(false);
            }
        }
    }

    /// <summary>
    /// Exits the game
    /// </summary>
    public void ExitGame()
    {
        Application.Quit();
    }

    /// <summary>
    /// Goes back to the main menu
    /// </summary>
    public void MainMenu()
    {
        SceneManager.LoadScene("Main menu");
    }
}
