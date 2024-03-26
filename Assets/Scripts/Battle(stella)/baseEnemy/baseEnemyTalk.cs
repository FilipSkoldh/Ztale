using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class baseEnemyTalk : MonoBehaviour
{
    [SerializeField] private List<string> enemyLines = new List<string>();
    [SerializeField] private GameObject speechBubble;

    public void Talk(int line)
    {
        speechBubble.SetActive(true);
        speechBubble.GetComponentInChildren<TextMeshProUGUI>().text = enemyLines[line];
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
