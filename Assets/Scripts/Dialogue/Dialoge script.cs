using Subtegral.DialogueSystem.DataContainers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine;
using System.Threading;

public class Dialogescript : MonoBehaviour
{
    [SerializeField] public DialogueContainer dialogueContainer;
    [SerializeField] public TextMeshProUGUI Textmesh;
    private NodeLinkData nodeLinkData;
    private DialogueNodeData NodeData;
    private int NodeNumber = 0;
    private InputAction interact;
    public InputActionAsset actions;

    void OnEnable()
    {
        actions.FindActionMap("char").Enable();
    }
    private void Awake()
    {
        nodeLinkData = dialogueContainer.NodeLinks[0];
        NodeData = dialogueContainer.DialogueNodeData[0];
        interact = actions.FindActionMap("char").FindAction("interact");

    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(nodeLinkData.BaseNodeGUID);
        Debug.Log(nodeLinkData.TargetNodeGUID);
        Debug.Log(nodeLinkData.PortName);
        if (interact.WasPressedThisFrame())
        {

            NodeDialogue = DialogueNodeData;
            string text = NodeDialogue.DialogueText;
            Textmesh.text = text;
            NodeNumber++;
            if (NodeNumber == 1 )
            {
                NodeNumber = 0;
            }
        }
    }

}
