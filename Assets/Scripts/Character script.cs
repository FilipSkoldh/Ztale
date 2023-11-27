using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public class Characterscript : MonoBehaviour
{
    public InputActionAsset actions;
    private InputAction moveaction;




    private void Awake()
    {
        moveaction = actions.FindActionMap("char").FindAction("move");
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 moveVector = moveaction.ReadValue<Vector2>();
    }
}
