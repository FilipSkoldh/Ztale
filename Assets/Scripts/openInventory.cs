using QuantumTek.QuantumInventory;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class openInventory : MonoBehaviour
{
    [SerializeField] private InputActionProperty openInv;
    [SerializeField] private GameObject invGameobj;
    [SerializeField] private EventSystem eventSystem;
    [SerializeField] private List<GameObject> invList;
    public QI_Inventory inventory;
    public QI_ItemDatabase itemDatabase;
    private Dictionary<string, QI_ItemData> items = new Dictionary<string, QI_ItemData>();
    private List<string> itemList = new List<string>();


    // Start is called before the first frame update
    void Start()
    {
        itemList = inventory.GetItems();

        for (int i = 0; i < itemList.Count; i++)
        {
            items.Add(itemList[i], itemDatabase.GetItem(itemList[i]));
        }
        items.Add("Shotgun", itemDatabase.GetItem("Shotgun"));
        items.Add("Bandages", itemDatabase.GetItem("Bandages"));
        inventory.AddItem(items["Shotgun"], 1);
        inventory.AddItem(items["Bandages"], 5);
    }
    private void invRefresh()
    {
        for (int i = 0; i < inventory.Stacks.Count; i++)
        {
            GameObject itemRefresh = invList[i];
            itemRefresh.SetActive(true);
            
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
            if (invGameobj.activeSelf)
            {
                invGameobj.SetActive(false);
            }
            else
            {
                invGameobj.SetActive(true);
                if (invGameobj.transform.GetChild(12).gameObject.activeSelf)
                  eventSystem.SetSelectedGameObject(invGameobj.transform.GetChild(12).GetChild(0).gameObject);
                invRefresh();
            }
        }
        if (invGameobj.activeSelf)
        {


        }
    }
}
