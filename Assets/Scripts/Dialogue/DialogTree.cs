using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogResponse
{
    public int nexBranchID;
    public string responseDialog;
}
[System.Serializable]
public class DialogSection
{
    public DialogResponse[] responses;
    public string dialog;
}
[System.Serializable]
public class DialogBranch
{
    public string branchName;
    public int branchID;
    public DialogSection[] sections;
    public bool endOnFinal;
}

public class DialogTree : MonoBehaviour
{
    public DialogBranch[] branches;
    public string NPCName;
}
