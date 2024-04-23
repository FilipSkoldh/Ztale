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


		List<Button> buttons = new(); 
		// Display all the choices, if there are any!
		if(inkStory.currentChoices.Count > 0) {
			for (int i = 0; i < inkStory.currentChoices.Count; i++) {
				Choice choice = inkStory.currentChoices [i];
				Button button = CreateChoiceView (choice.text.Trim ());
				buttons.Add (button);
				// Tell the button what to do when we press it
				button.onClick.AddListener (delegate {
					OnClickChoiceButton (choice);
				});
			}
		}

		if (buttons.Count > 0) 
		{ 
			eventSystem.SetSelectedGameObject(buttons[0].gameObject);
		}
		else
		{
			RemoveChildren () ;
		}
		if(inkStory.currentChoices.Count == 1)
		{
			if (inkStory.currentChoices[0].text == ".")
			{
                buttons[0].transform.localPosition = new Vector3(-2000, 315, 0);
            }
			else
			{
                buttons[0].transform.localPosition = new Vector3(-590, 315, 0);
            }

        }
        else if (inkStory.currentChoices.Count == 2)
        {
            buttons[0].transform.localPosition = new Vector3(-590, 315, 0);
            buttons[1].transform.localPosition = new Vector3(250, 315, 0);
        }
        else if (inkStory.currentChoices.Count == 3)
        {
            buttons[0].transform.localPosition = new Vector3(-590, 315, 0);
            buttons[1].transform.localPosition = new Vector3(-150, 315, 0);
            buttons[2].transform.localPosition = new Vector3(270, 315, 0);
        }
        // If we've read all the content and there's no choices, the story is finished!

    }

	// When we click the choice button, tell the story to choose that choice!
	void OnClickChoiceButton (Choice choice) 
	{
		inkStory.ChooseChoiceIndex (choice.index);
		RefreshView();
	}

	// Creates a textbox showing the the line of text
	void CreateContentView (string text)
	{
		GameObject storyText = Instantiate (textPrefab, canvas.transform.TransformPoint(0, 384.5f, 0), Quaternion.identity, canvas.transform);
		storyText.GetComponentInChildren<TextMeshProUGUI>().text = text;
		storyText.transform.SetParent (canvas.transform, false);
	}

	// Creates a button showing the choice text
	Button CreateChoiceView (string text) 
	{
		// Creates the button from a prefab
		Button choice = Instantiate (buttonPrefab, canvas.transform);
		choice.transform.SetParent (canvas.transform, false);
		
		// Gets the text from the button prefab
		TextMeshProUGUI choiceText = choice.GetComponentInChildren<TextMeshProUGUI> ();
		choiceText.text = text;

		return choice;
	}

	// Destroys all the children of this gameobject (all the UI)
	void RemoveChildren () 
	{
		int childCount = canvas.transform.childCount;
		for (int i = childCount - 1; i >= 0; --i) {
			Destroy (canvas.transform.GetChild (i).gameObject);
		}
	}

	void StartEncounter(int encounter)
	{
		GlobalVariables.Encounter = encounter;
        GlobalVariables.PlayerInventory = inventory.Stacks;
        SceneManager.LoadScene("Battle");
	}



    public TextAsset inkJSONAsset = null;
	public Story inkStory;

	[SerializeField]
	private Canvas canvas = null;
	[SerializeField]
	private EventSystem eventSystem = null;
	// UI Prefabs
	[SerializeField]
	private GameObject textPrefab = null;
	[SerializeField]
	private Button buttonPrefab = null;
	[SerializeField] private InteractWithChest chest;
	[SerializeField] private QI_Inventory inventory;
}
