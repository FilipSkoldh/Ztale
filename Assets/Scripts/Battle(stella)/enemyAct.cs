using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyAct : MonoBehaviour
{
    public List<string> acts = new List<string>();
    public List<string> spareActs = new List<string>();

    [SerializeField] private enemyLife relay;
    // Start is called before the first frame update

    public void Act(string action)
    {
        if (action == spareActs[spareActs.Count])
        {

        }
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
}
