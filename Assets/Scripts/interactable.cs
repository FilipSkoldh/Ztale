using Ink.Parsed;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class interactable : MonoBehaviour
{
    private TextAsset inkJSONAsset = null;
    public Story story;
    private void Start()
    {
        story = new Story(inkJSONAsset.text);
    }
}
