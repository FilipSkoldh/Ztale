using QuantumTek.QuantumInventory;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class openInventory : MonoBehaviour
{
    [SerializeField] private InputActionProperty openInv;
    [SerializeField] private GameObject inventoryGUI;
    [SerializeField] private EventSystem eventSystem;
    [SerializeField] private List<GameObject> invList;
    public QI_Inventory inventory;
    public QI_ItemDatabase itemDatabase;
    private Dictionary<string, QI_ItemData> items = new Dictionary<string, QI_ItemData>();


    // Start is called before the first frame update
    void Start()
    {
        items = itemDatabase.Getdictionary();
        inventory.AddItem(items["Shotgun"], 1);
        inventory.AddItem(items["Bandages"], 5);
    }
    private void invRefresh()
    {
        for (int i = 0; i < inventory.Stacks.Count; i++)
        {
            GameObject itemRefresh = invList[i];
            
            if (inventory.GetStock(inventory.Stacks[i].Item.Name) != itemDatabase.GetItem(inventory.Stacks[i].Item.Name).MaxStack)
            {
                itemRefresh.GetComponent<TextMeshProUGUI>().text = $"- {inventory.Stacks[i].Item.Name} x{inventory.GetStock(inventory.Stacks[i].Item.Name)}";
            }
            else
            {
                itemRefresh.GetComponent<TextMeshProUGUI>().text = $"- {inventory.Stacks[i].Item.Name}";
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (openInv.action.WasPressedThisFrame())
        {
            if (inventoryGUI.activeSelf)
            {
                inventoryGUI.SetActive(false);
            }
            else
            {
                inventoryGUI.SetActive(true);
                if (inventoryGUI.transform.GetChild(12).gameObject.activeSelf)
                  eventSystem.SetSelectedGameObject(inventoryGUI.transform.GetChild(12).GetChild(0).gameObject);
                invRefresh();
            }
        }
        if (inventoryGUI.activeSelf)
        {


        }
    }
}
