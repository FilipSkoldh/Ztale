using QuantumTek.QuantumInventory;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting.InputSystem;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class InteractWithChest : MonoBehaviour
{
    [SerializeField] private OpenInventory openInventory;
    [SerializeField] private GameObject chestGameobj;
    [SerializeField] private EventSystem eventSystem;
    [SerializeField] private List<GameObject> invList;
    [SerializeField] private SaveAndLoad saveAndLoad;
    [SerializeField] private InputActionProperty inv;
    [SerializeField] private InputActionProperty close;
    private QI_Inventory inventory;
    public QI_ItemDatabase itemDatabase;
    private Dictionary<string, QI_ItemData> items = new Dictionary<string, QI_ItemData>();
    private void Start()
    {
        items = itemDatabase.Getdictionary();
    }

    public void OpenChest(int inventoryNumber)
    {
        inventory = saveAndLoad.inventories[inventoryNumber];
        inventory.AddItem(items["Bandages"], 5);
        chestGameobj.SetActive(true);
        openInventory.OpenInv();


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

    private void Update()
    {
        
    }
}
