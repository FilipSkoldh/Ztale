using QuantumTek.QuantumInventory;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class Inventory : MonoBehaviour
{
    [SerializeField] private InputActionProperty openInv;
    [SerializeField] private GameObject invGameobj;
    [SerializeField] private EventSystem eventSystem;
    public QI_Inventory inventory;
    public QI_ItemDatabase itemDatabase;
    private bool pressedlastframe;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (openInv.action.WasPressedThisFrame())
        {
            if (invGameobj.activeSelf)
            {
                invGameobj.SetActive(false);
            }
            else
            {
                invGameobj.SetActive(true);
                if (invGameobj.transform.GetChild(12).gameObject.activeSelf)
                  eventSystem.SetSelectedGameObject(invGameobj.transform.GetChild(12).GetChild(0).gameObject);

            }
        }
        if (invGameobj.activeSelf)
        {

        }
    }
}
