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
    public InputActionProperty interact;
    public int nodelinkss;
    public int dialognod;
    

    private void Awake()
    {

    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        nodeLinkData = dialogueContainer.NodeLinks[nodelinkss];
        NodeData = dialogueContainer.DialogueNodeData[dialognod];
        Debug.Log(nodeLinkData.BaseNodeGUID);
        Debug.Log(nodeLinkData.TargetNodeGUID);
        Debug.Log(nodeLinkData.PortName);
        
        

            string text = NodeData.DialogueText;
            Textmesh.text = text;
            NodeNumber++;
            if (NodeNumber == 1 )
            {
                NodeNumber = 0;
            }
        
    }

}
