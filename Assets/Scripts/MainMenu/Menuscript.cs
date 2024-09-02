using Newtonsoft.Json;
using System;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Menuscript : MonoBehaviour
{
    //Declaring variables

    //all inputs needed in the script
    [SerializeField] private InputActionProperty x;
    [SerializeField] private InputActionProperty ctrl;
    [SerializeField] private InputActionProperty enter;

    //Parts of the UI
    [SerializeField] TMP_InputField nameInputField;
    [SerializeField] GameObject[] mainMenu = new GameObject[8];

    //The scenes EventSystem
    [SerializeField] EventSystem eventSystem;

    //which savefiles page is selected
    int curretlySelectedPage = 1;

    // if we're currently selecting a savefile
    bool currentlySelectingSavefile = false;

    //if we're currently typing
    bool typing = false;

    //the path where savefiles are located
    string savefilePath = $"{Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)}\\Documents\\My Games\\Ztale\\Saves";

    //create a Savefile variable to load data
    Savefile savefile = new();

    private void Start()
    {
        MainMenu();
    }

    private void Update()
    {
        //if x is pressed or ctrl + x while typing go back to main menu
        if ((!typing && x.action.WasPressedThisFrame()) || (x.action.WasPressedThisFrame() && ctrl.action.IsPressed()))
        {
            //Go back to mainMenu
            MainMenu();
        }

        //if enter is pressed while typing create the save foldes if it doesn't exist and save the name that the use typed and which savefileNumber this new save has then load "Overworld" scene
        if (enter.action.WasPressedThisFrame() && typing)
        {
            Directory.CreateDirectory($"{Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)}\\Documents\\My Games");
            Directory.CreateDirectory($"{Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)}\\Documents\\My Games\\Ztale");
            Directory.CreateDirectory(savefilePath);
            GlobalVariables.PlayerName = nameInputField.text;
            GlobalVariables.Savefile = Directory.GetFiles(savefilePath, "*", SearchOption.TopDirectoryOnly).Length;
            SceneManager.LoadScene("Overworld");
        }

    }


    /// <summary>
    /// Load the MainMenu
    /// </summary>
    public void MainMenu()
    {
        //set all part of UI to active
        foreach (var menu in mainMenu)
            menu.gameObject.SetActive(true);

        //Disable previous and next page buttons and the nameInputField
        mainMenu[5].gameObject.SetActive(false);
        mainMenu[6].gameObject.SetActive(false);
        nameInputField.gameObject.SetActive(false);

        //set the buttons do the display correct text
        mainMenu[1].GetComponent<TextMeshProUGUI>().text = "- New save";
        mainMenu[2].GetComponent<TextMeshProUGUI>().text = "- Load save";
        mainMenu[3].GetComponent<TextMeshProUGUI>().text = "- Settings";
        mainMenu[4].GetComponent<TextMeshProUGUI>().text = "- Exit game";
        //select first button in the menu
        eventSystem.SetSelectedGameObject(mainMenu[1].transform.GetChild(0).gameObject);
        //resets "CurrentlySelectingSavefile" and "GlobalVariables.LoadedSave"
        currentlySelectingSavefile = false;
        GlobalVariables.LoadedSave = false;
        GlobalVariables.Playerdead = false;
    }

    /// <summary>
    /// Create and name a new savefile
    /// </summary>
    public void NewSave()
    {
        //If CurrentlySelectingSavefile is true don't create a new save
        if (!currentlySelectingSavefile)
        {
            //disables everying in the UI
            foreach (var menu in mainMenu)
                menu.gameObject.SetActive(false);

            //enables the nameInputField and selects it
            nameInputField.gameObject.SetActive(true);
            eventSystem.SetSelectedGameObject(nameInputField.gameObject);
            typing = true;
        }
    }


    /// <summary>
    /// Load the savefiles page
    /// </summary>
    /// <param name="loadPage">Which savefile page to load</param>
    public void LoadSaveFiles(int loadPage)
    {
        //Disable the "coming soon" and enables the previous and next page buttons
        mainMenu[7].gameObject.SetActive(false);
        mainMenu[5].gameObject.SetActive(true);
        mainMenu[6].gameObject.SetActive(true);

        //if "currentlySelectingSavefile" is false load in the savefiles in on the selcted page
        if (!currentlySelectingSavefile)
        {
            //j is the menu button which is going to be changed
            int j = 1;
            //i = 0 for page 1 and goes to 3, i = 4 for page 2 goes to 7 and so on
            for (int i = 4 * loadPage; i < 4 + 4*loadPage; i++)
            {
                //display the name of the savefile if it exists otherwise display "Empty"
                if (File.Exists($"{savefilePath}\\Save{i}"))
                {
                    savefile = JsonConvert.DeserializeObject<Savefile>(File.ReadAllText($"{savefilePath}\\Save{i}"));
                    mainMenu[j].GetComponent<TextMeshProUGUI>().text = $"- {savefile.playerName}";
                }
                else
                {
                    mainMenu[j].GetComponent<TextMeshProUGUI>().text = $"- Empty";
                }
                j++;
            }
            //when all savefiles are displayed set currentlySelectingSavefile to true
            currentlySelectingSavefile = true;
        }
        curretlySelectedPage = loadPage;
    }


    /// <summary>
    /// Go to next savefiles Page
    /// </summary>
    public void NextPage()
    {
        currentlySelectingSavefile = false;
        LoadSaveFiles(curretlySelectedPage + 1);
    }


    /// <summary>
    /// Go to previous safefiles page
    /// </summary>
    public void PreviousPage()
    {
        currentlySelectingSavefile = false;
        //don't go back if we're on page 1
        if (curretlySelectedPage != 0)
        {
            LoadSaveFiles(curretlySelectedPage - 1);
        }
    }

    /// <summary>
    /// Loads the selected savefile
    /// </summary>
    /// <param name="saveslot">The savefile to be loaded</param>
    public void LoadSave(int saveslot)
    {
        //only load save if "currentlySelectingSavefile"
        if (currentlySelectingSavefile)
        {
            GlobalVariables.Savefile = saveslot + curretlySelectedPage * 4;
            SceneManager.LoadScene("Overworld");
        }
    }

    /// <summary>
    /// Close game
    /// </summary>
    public void Exit()
    {
        //if "currentlySelectingSavefile" = false
        if (!currentlySelectingSavefile)
        {
            Application.Quit();
        }
    }
}
