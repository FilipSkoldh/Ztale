using UnityEngine;
using Ink.Runtime;
using TMPro;
using UnityEngine.InputSystem;

public class DialogueScritp : MonoBehaviour
{
    public TextAsset inkAsset;
    public TextMeshProUGUI tmp;
    public InputActionProperty e;

    private string text;

    Story _inkstory;

    private void Awake()
    {
        _inkstory = new Story(inkAsset.text);
        text = Story.
    }


    private void Update()
    {
        tmp.text = text; 
    }
}
