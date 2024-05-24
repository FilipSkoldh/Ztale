using System;
using System.Collections.Generic;
using Ink.Runtime;
using QuantumTek.QuantumInventory;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InkScript: MonoBehaviour {
    public static event Action<Story> OnCreateStory;
	
	// Creates a new Story object with the compiled story which we can then play!
	public void StartStory () {
		inkStory = new Story (inkJSONAsset.text);
        if (OnCreateStory != null)
		{
			OnCreateStory(inkStory);
		}
        inkStory.BindExternalFunction("OpenChest", (int chestNumber) => {chest.OpenChest(chestNumber); });
		inkStory.BindExternalFunction("StartEncounter", (int encounter) => { StartEncounter(encounter); });
		inkStory.BindExternalFunction("SaveGame", (int savelocation) => { saveAndLoad.Save(savelocation); });
		inkStory.BindExternalFunction("FillInventory", () => { saveAndLoad.FillInventories();});
		RefreshView();
	}
	
	/// <summary>
	/// Destroy old text and choices the continues the story and displays all choices, if there are none the story is finished
	/// </summary>
	void RefreshView () 
	{

		RemoveChildren ();

		// Read all the content until we can't continue any more
		while (inkStory.canContinue) {
			// Continue gets the next line of the story
			string text = inkStory.Continue ();
			// This removes any white space from the text.
			text = text.Trim();
			// Display the text on screen!
			CreateContentView(text);
		}

		//A list with the Choice buttons
		List<Button> buttons = new(); 

		// Display all the choices, if there are any!
		if(inkStory.currentChoices.Count > 0) {
			for (int i = 0; i < inkStory.currentChoices.Count; i++) {
				Choice choice = inkStory.currentChoices [i];
				Button button = CreateChoiceView (choice.text.Trim ());
				//Adds the current choice button to the list
				buttons.Add (button);
				// Tell the button what to do when we press it
				button.onClick.AddListener (delegate {OnClickChoiceButton (choice);});
			}
		}

		//if there are any choices at all select the first button
		if (buttons.Count > 0) 
			eventSystem.SetSelectedGameObject(buttons[0].gameObject);
		else
			RemoveChildren () ;

		//Where to place the choice buttons depending on how many choices there are
		if(inkStory.currentChoices.Count == 1)
		{
			//If there is only one choice and that is a "." that is just so you have to click to further the dialoge so place the "." choice outside the screen
			if (inkStory.currentChoices[0].text == ".")
			{
                buttons[0].GetComponent<RectTransform>().anchoredPosition = new Vector3(-590, 200, 0);
            }
            //Else place the button at the specified location
            else
			{
                buttons[0].GetComponent<RectTransform>().anchoredPosition = new Vector3(-590, -220, 0);
            }

        }
        else if (inkStory.currentChoices.Count == 2)
        {
			//where to place buttons when there are two choices
            buttons[0].GetComponent<RectTransform>().anchoredPosition = new Vector3(-590, -220, 0);
            buttons[1].GetComponent<RectTransform>().anchoredPosition = new Vector3(250, -220, 0);

        }
        else if (inkStory.currentChoices.Count == 3)
        {
			//where to place buttons when there are three choices
            buttons[0].GetComponent<RectTransform>().anchoredPosition = new Vector3(-590, -220, 0);
            buttons[1].GetComponent<RectTransform>().anchoredPosition = new Vector3(-150, -220, 0);
            buttons[2].GetComponent<RectTransform>().anchoredPosition = new Vector3(270, -220, 0);
        }
        // If we've read all the content and there's no choices, the story is finished!
    }

	/// <summary>
	/// Tells story which choice was chosen
	/// </summary>
	/// <param name="choice">The choice to tell Story</param>
	void OnClickChoiceButton (Choice choice) 
	{
		inkStory.ChooseChoiceIndex (choice.index);
		RefreshView();
	}

    /// <summary>
    /// Creates a textbox showing the the line of text
    /// </summary>
    /// <param name="text">The text to be displayed</param>
    void CreateContentView (string text)
	{
        //spawn the textbox prefab
        GameObject storyText = Instantiate(textboxPrefab, new Vector3(0, -150, 0), Quaternion.identity, textboxCanvas.transform);
        storyText.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, -150, 0);
        //text the TextmeshproUGUI to "text"
        storyText.GetComponentInChildren<TextMeshProUGUI>().text = text;
    }

	/// <summary>
	/// Creates a button showing the choice text
	/// </summary>
	/// <param name="text">The choice text</param>
	/// <returns></returns>
	Button CreateChoiceView (string text) 
	{
		// Creates the button from a prefab
		Button choice = Instantiate (buttonPrefab, textboxCanvas.transform);
		choice.transform.SetParent (textboxCanvas.transform, false);
		
		// Gets the text from the button prefab
		TextMeshProUGUI choiceText = choice.GetComponentInChildren<TextMeshProUGUI> ();
		choiceText.text = text;

		return choice;
	}

	/// <summary>
	/// Destroys all UI gameobjects
	/// </summary>
	void RemoveChildren () 
	{
        //Destroy all children in the textboxcanvas to remove the textbox and choices
        for (int i = textboxCanvas.transform.childCount - 1; i >= 0; --i) 
			Destroy (textboxCanvas.transform.GetChild (i).gameObject);
	}

	/// <summary>
	/// Starts a battle encounter
	/// </summary>
	/// <param name="encounter">The encounter to be started</param>
	void StartEncounter(int encounter)
	{
		//Tells the other scene which encounter to start and the players inventory
		GlobalVariables.Encounter = encounter;
		saveAndLoad.SaveScene();
		//switches scenes
        SceneManager.LoadScene("Battle");
	}



    public TextAsset inkJSONAsset = null;
	public Story inkStory;

	[SerializeField]
	private Canvas textboxCanvas = null;
	[SerializeField]
	private EventSystem eventSystem = null;
	// UI Prefabs
	[SerializeField]
	private GameObject textboxPrefab = null;
	[SerializeField]
	private Button buttonPrefab = null;
	[SerializeField] private InteractWithChest chest;
	[SerializeField] private QI_Inventory inventory;
	[SerializeField] private SaveAndLoad saveAndLoad;
}
