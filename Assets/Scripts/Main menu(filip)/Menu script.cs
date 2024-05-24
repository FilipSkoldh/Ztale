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
    [SerializeField] private InputActionProperty x;
    [SerializeField] private InputActionProperty ctrl;
    [SerializeField] private InputActionProperty enter;
    [SerializeField] TMP_InputField nameInputField;
    [SerializeField] GameObject[] mainMenu = new GameObject[8];
    [SerializeField] Canvas canvas;
    [SerializeField] EventSystem eventSystem;
    int page = 1;
    bool selectingSave = false;
    bool typing = false;
    string savefilePath = $"{Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)}\\Documents\\My Games\\Ztale\\Saves";
    Savefile savefile = new();

    private void Start()
    {
        MainMenu();
    }

    private void Update()
    {
        if ((!typing && x.action.WasPressedThisFrame()) || (x.action.WasPressedThisFrame() && ctrl.action.IsPressed()))
        {
            eventSystem.SetSelectedGameObject(mainMenu[1].GetComponent<GameObject>());
            Debug.Log("yes");
            MainMenu();
        }

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

    public void MainMenu()
    {
        foreach (var menu in mainMenu)
            menu.gameObject.SetActive(true);

        mainMenu[5].gameObject.SetActive(false);
        mainMenu[6].gameObject.SetActive(false);

        nameInputField.gameObject.SetActive(false);
        mainMenu[1].GetComponent<TextMeshProUGUI>().text = "- New save";
        mainMenu[2].GetComponent<TextMeshProUGUI>().text = "- Load save";
        mainMenu[3].GetComponent<TextMeshProUGUI>().text = "- Settings";
        mainMenu[4].GetComponent<TextMeshProUGUI>().text = "- Exit game";
        eventSystem.SetSelectedGameObject(mainMenu[1].transform.GetChild(0).gameObject);
        selectingSave = false;
        GlobalVariables.LoadedSave = false;
    }
    public void NewSave()
    {
        if (!selectingSave)
        {
            foreach (var menu in mainMenu)
                menu.gameObject.SetActive(false);

            nameInputField.gameObject.SetActive(true);
            eventSystem.SetSelectedGameObject(nameInputField.gameObject);
        }
        typing = true;
    }



    public void LoadSaveFiles(int loadPage)
    {
        mainMenu[7].gameObject.SetActive(false);
        mainMenu[5].gameObject.SetActive(true);
        mainMenu[6].gameObject.SetActive(true);
        

        if (!selectingSave)
        {
            int j = 1;
            for (int i = 4*loadPage - 3; i < 1 + 4 * loadPage; i++)
            {
                if (File.Exists($"{savefilePath}\\Save{i - 1}"))
                {
                    savefile = JsonConvert.DeserializeObject<Savefile>(File.ReadAllText($"{savefilePath}\\Save{i-1}"));
                    mainMenu[j].GetComponent<TextMeshProUGUI>().text = $"- {savefile.playerName}";
                }
                else
                {
                    mainMenu[j].GetComponent<TextMeshProUGUI>().text = $"- Empty";
                }
                j++;
            }
            selectingSave = true;
        }
        page = loadPage;
    }

    public void NextPage()
    {
        selectingSave = false;
        LoadSaveFiles(page+1);
    }
    public void PreviousPage()
    {
        selectingSave = false;
        if (page != 1)
        {
            LoadSaveFiles(page-1);
        }
    }

    public void LoadSave(int saveslot)
    {
        if (selectingSave)
        {
            GlobalVariables.Savefile = saveslot * page;
            SceneManager.LoadScene("Overworld");
        }
    }

    public void Exit()
    {
        if (!selectingSave)
        {
            Application.Quit();
        }
    }
}
