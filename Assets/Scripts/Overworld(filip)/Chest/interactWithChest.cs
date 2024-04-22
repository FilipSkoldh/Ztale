using QuantumTek.QuantumInventory;
using System.Collections.Generic;
using TMPro;
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
    private Dictionary<string, QI_ItemData> items = new();
    QI_Chest chestVendor;
    private void Start()
    {
        items = itemDatabase.Getdictionary();
    }

    public void OpenChest(int inventoryNumber)
    {
        inventory = saveAndLoad.chests[inventoryNumber];
        inventory.AddItem(items["Bandage"], 10);
        inventory.AddItem(items["Pistol"], 1);
        chestGameobj.SetActive(true);
        openInventory.OpenOrCloseInventory();
        chestVendor = saveAndLoad.transferChests[inventoryNumber+1];

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
        if (inventory.Stacks.Count > button)
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
