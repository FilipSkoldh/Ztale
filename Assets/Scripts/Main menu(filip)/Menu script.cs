using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.Composites;
using UnityEngine.SceneManagement;

public class Menuscript : MonoBehaviour
{
    [SerializeField] GameObject[] mainMenu = new GameObject[4];
    bool selectingSave = false;
    private void Start()
    {
        mainMenu[0].GetComponent<TextMeshProUGUI>().text = "- New save";
        mainMenu[1].GetComponent<TextMeshProUGUI>().text = "- Load save";
        mainMenu[2].GetComponent<TextMeshProUGUI>().text = "- Settings";
        mainMenu[3].GetComponent<TextMeshProUGUI>().text = "- Exit game";
        selectingSave = false;
    }
    public void CreateOrLoad()
    {
        if (!selectingSave)
        {
            mainMenu[0].GetComponent<TextMeshProUGUI>().text = "- Empty";
            mainMenu[1].GetComponent<TextMeshProUGUI>().text = "- Empty";
            mainMenu[2].GetComponent<TextMeshProUGUI>().text = "- Empty";
            mainMenu[3].GetComponent<TextMeshProUGUI>().text = "- Empty";
            selectingSave = true;
        }
    }

    public void StartGame(int savefile)
    {

        GlobalVariables.savefile = savefile;
        SceneManager.LoadScene("Overworld");

    }
}
