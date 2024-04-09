using QuantumTek.QuantumInventory;
using System.Collections.Generic;
using System.Transactions;
using TMPro;
using Unity.VisualScripting.InputSystem;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class InteractWithChest : MonoBehaviour
{
    [SerializeField] private InteractWithInventory openInventory;
    [SerializeField] private GameObject chestGameobj;
    [SerializeField] private EventSystem eventSystem;
    [SerializeField] private List<GameObject> invList;
    [SerializeField] private SaveAndLoad saveAndLoad;
    [SerializeField] private InputActionProperty inv;
    [SerializeField] private InputActionProperty close;
    [SerializeField] private QI_Chest playerChest;
    private QI_Inventory inventory;
    public QI_ItemDatabase itemDatabase;
    private Dictionary<string, QI_ItemData> items = new Dictionary<string, QI_ItemData>();
    QI_Chest chestVendor;
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
        chestVendor = saveAndLoad.chests[inventoryNumber];
        RefreshChest();
    }

    public void RefreshChest()
    {
        foreach (var item in invList)
        {
            item.GetComponent<TextMeshProUGUI>().text = "-";
        }

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

    public void MoveFromChest(int button)
    {
        if (inventory.Stacks.Count >= button)
        {
            QI_Chest.Transaction(playerChest, chestVendor, inventory.Stacks[button].Item, 1);
            RefreshChest();
            openInventory.RefreshInventory();
        }
    }


    private void Update()
    {
        if (inv.action.WasPressedThisFrame() || close.action.WasPressedThisFrame())
        {
            chestGameobj.SetActive(false);
        }
        if (chestGameobj.activeSelf)
        {

        }
    }
}
