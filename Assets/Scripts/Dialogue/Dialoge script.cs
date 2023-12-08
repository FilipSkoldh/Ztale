using Subtegral.DialogueSystem.DataContainers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine;

public class Dialogescript : MonoBehaviour
{
    [SerializeField] public DialogueContainer dialogueContainer;
    [SerializeField] public TextMeshProUGUI Textmesh;
    private NodeLinkData nodeLinkData;
    private DialogueNodeData nodeData;
    private InputAction interact;
    public InputActionAsset actions;

    void OnEnable()
    {
        actions.FindActionMap("char").Enable();
    }
    private void Awake()
    {
        nodeLinkData = dialogueContainer.NodeLinks[0];
        interact = actions.FindActionMap("char").FindAction("interact");

    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        nodeData = dialogueContainer.DialogueNodeData[0];
        string text = nodeData.DialogueText;
        Debug.Log(text);
        if (interact.IsPressed())
        {
            Textmesh.text = text;
        }
    }

}
