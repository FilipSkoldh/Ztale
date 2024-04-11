using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleManager : MonoBehaviour
{
    public Scene overWorld;
    // Start is called before the first frame update
    void Start()
    {
            SceneManager.LoadScene(overWorld.name);
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.childCount == 0)
        {
            SceneManager.SetActiveScene(overWorld);
        }
    }
}
