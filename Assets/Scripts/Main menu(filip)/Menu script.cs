using System;
using System.IO;

using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Menuscript : MonoBehaviour
{
    [SerializeField] TMP_InputField nameInputField;
    [SerializeField] GameObject[] mainMenu = new GameObject[5];
    [SerializeField] Canvas canvas;
    [SerializeField] EventSystem eventSystem;
    bool selectingSave = false;
    private void Start()
    {
        mainMenu[1].GetComponent<TextMeshProUGUI>().text = "- New save";
        mainMenu[2].GetComponent<TextMeshProUGUI>().text = "- Load save";
        mainMenu[3].GetComponent<TextMeshProUGUI>().text = "- Settings";
        mainMenu[4].GetComponent<TextMeshProUGUI>().text = "- Exit game";
        selectingSave = false;
    }
    public void NewSave()
    {
        foreach (var menu in mainMenu)
            menu.gameObject.SetActive(false);
        
        nameInputField.gameObject.SetActive(true);
        eventSystem.SetSelectedGameObject(nameInputField.gameObject);
    }

    public void CreateNewSave()
    {
        GlobalVariables.SaveName = nameInputField.text;
        SceneManager.LoadScene("Overworld");
    }
    public void LoadSavefile()
    {
        if (!selectingSave)
        {
            mainMenu[1].GetComponent<TextMeshProUGUI>().text = "- Empty";
            mainMenu[2].GetComponent<TextMeshProUGUI>().text = "- Empty";
            mainMenu[3].GetComponent<TextMeshProUGUI>().text = "- Empty";
            mainMenu[4].GetComponent<TextMeshProUGUI>().text = "- Empty";
            selectingSave = true;
        }
    }

}
