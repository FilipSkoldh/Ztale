using QuantumTek.QuantumInventory;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class InteractWithChest : MonoBehaviour
{
    //Declares variables

    //All inputs needed
    [SerializeField] private InputActionProperty inv;
    [SerializeField] private InputActionProperty close;

    //Chest GUI
    [SerializeField] private GameObject chestGameobj;

    //Chests inventory slots and the EventSystem
    [SerializeField] private List<GameObject> invList;
    [SerializeField] private EventSystem eventSystem;

    //Chests inventory and chest moving script
    private QI_Inventory inventory;
    QI_Chest chestVendor;

    //the database and dictionary with all item info
    public QI_ItemDatabase itemDatabase;
    private Dictionary<string, QI_ItemData> items = new();

    //Other scripts
    [SerializeField] private InteractWithInventory openInventory;
    [SerializeField] private SaveAndLoad saveAndLoad;
    [SerializeField] private QI_Chest playerChestVendor;

    private void Awake()
    {
        items = itemDatabase.Getdictionary();
        inventory.AddItem(items["Bandage"], 10);
        inventory.AddItem(items["Pistol"], 1);        
    }

    public void OpenChest(int inventoryNumber)
    {
        chestVendor = saveAndLoad.transferChests[inventoryNumber+1];
        inventory = saveAndLoad.chests[inventoryNumber];
        chestGameobj.SetActive(true);
        openInventory.OpenOrCloseInventory();

        RefreshChest();
    }

    public void RefreshChest()
    {
        //Resets all textboxes in the chest GUI
        foreach (var item in invList)
        {
            item.GetComponent<TextMeshProUGUI>().text = "-";
        }
        //Sorts the chests inventory in alphabetical order
        inventory.Stacks.Sort((p1, p2) => { return string.Compare(p1.Item.name, p2.Item.name); });

        //Reads the chests inventory and puts it in the corresponding positions in the inventory GUI
        //for every stack in the inventory
        for (int i = 0; i < inventory.Stacks.Count; i++)
        {
            //If the item is stackable write out the name of the item and the amount
            if (itemDatabase.GetItem(inventory.Stacks[i].Item.Name).MaxStack != 1)
            {
                invList[i].GetComponent<TextMeshProUGUI>().text = $"- {inventory.Stacks[i].Item.Name} x{inventory.Stacks[i].Amount}";
            }
            //Else write out only the name
            else
            {
                invList[i].GetComponent<TextMeshProUGUI>().text = $"- {inventory.Stacks[i].Item.Name}";
            }
        }
    }

    public void MoveFromChest(int button)
    {
        if (inventory.Stacks.Count > button)
        {
            QI_Chest.Transaction(playerChestVendor, chestVendor, inventory.Stacks[button].Item, 1);
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
