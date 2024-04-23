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

    //The database and dictionary with all item info
    public QI_ItemDatabase itemDatabase;
    private Dictionary<string, QI_ItemData> items = new();

    //Other scripts
    [SerializeField] private InteractWithInventory openInventory;
    [SerializeField] private SaveAndLoad saveAndLoad;
    [SerializeField] private QI_Chest playerChestVendor;

    private void Awake()
    {
        //Get full Itemdatabase with all item info and add to dictionary
        items = itemDatabase.Getdictionary();
        //Use the dictionary to add items easily to the chests inventory
        inventory.AddItem(items["Bandage"], 10);
        inventory.AddItem(items["Pistol"], 1);        
    }

    /// <summary>
    /// Loads and openes a chest
    /// </summary>
    /// <param name="inventoryNumber">The chest to be opened</param>
    public void OpenChest(int inventoryNumber)
    {
        //Gets the chests chests inventory and chest moving script
        chestVendor = saveAndLoad.transferChests[inventoryNumber+1];
        inventory = saveAndLoad.chests[inventoryNumber];
        //Activates the chest GUI and openes the players inventory
        chestGameobj.SetActive(true);
        openInventory.OpenOrCloseInventory();

        RefreshChest();
    }

    /// <summary>
    /// Refreshes the chests GUI
    /// </summary>
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

    /// <summary>
    /// Moves an item from the chest inventory to the player inventory
    /// </summary>
    /// <param name="button">The item slot in the chests inventory to move</param>
    public void MoveFromChest(int button)
    {
        //Checks that you didn't press on an empty inventory slot
        if (inventory.Stacks.Count > button)
        {
            //Using QI_Chest moves the selected item to the player inventory then refreshes both the player inventory GUI and chest GUI
            QI_Chest.Transaction(playerChestVendor, chestVendor, inventory.Stacks[button].Item, 1);
            openInventory.RefreshInventory();
            RefreshChest();
        }
    }


    private void Update()
    {
        //if the inventory or close button was pressed close the chest
        if (inv.action.WasPressedThisFrame() || close.action.WasPressedThisFrame())
        {
            chestGameobj.SetActive(false);
        }
        if (chestGameobj.activeSelf)
        {

        }
    }
}
