using QuantumTek.QuantumInventory;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class InteractWithInventory : MonoBehaviour
{
    //Declares variables

    //All inputs needed
    [SerializeField] private InputActionProperty openInv;
    [SerializeField] private InputActionProperty close;
    [SerializeField] private InputActionProperty interact;

    //Inventory and chest GUI
    [SerializeField] private GameObject inventoryGUI;
    [SerializeField] private GameObject chestGUI;

    //the buttons behind every item in the inventory GUI and the eventsystem to control it
    [SerializeField] private List<GameObject> invList;
    [SerializeField] private EventSystem eventSystem;

    //The QI_Chest scripts on the chest and 
    [SerializeField] private QI_Chest chestVendor;
    private QI_Chest playerChestVendor;
    private InteractWithChest InteractWithChest;

    //the rest of the GUI
    [SerializeField] private GameObject textboxPrefab;
    [SerializeField] private Canvas textboxCanvas;
    [SerializeField] private TextMeshProUGUI uiHp;
    [SerializeField] private TextMeshProUGUI uiFood;
    [SerializeField] private TextMeshProUGUI uiWeapon;
    [SerializeField] private TextMeshProUGUI uiEquipment;
    [SerializeField] private TextMeshProUGUI uiLightAmmo;
    [SerializeField] private TextMeshProUGUI uiMediumAmmo;
    [SerializeField] private TextMeshProUGUI uiShotgunAmmo;

    //the database with all item info
    [SerializeField] private QI_ItemDatabase itemDatabase;

    //The inventory and the dictionary with names to all itemdata
    private QI_Inventory inventory;
    private Dictionary<string, QI_ItemData> items = new();

    //variables for inventory
    private int selectedItem = 0;
    private int wait = 0;
    private bool interacting = false;

    private void Awake()
    {
        //Finds components on the player
        inventory = GetComponent<QI_Inventory>();
        playerChestVendor = GetComponent<QI_Chest>();
        InteractWithChest = GetComponent<InteractWithChest>();

        //If there's a inventory saved globally load it
        if (GlobalVariables.PlayerInventory != null)
            inventory.Stacks = GlobalVariables.PlayerInventory;
        

    }
    // Start is called before the first frame update
    void Start()
    {
        //Get full Itemdatabase with all item info and add to dictionary
        items = itemDatabase.Getdictionary();
        //use the dictionary to add items easily to the inventory
        inventory.AddItem(items["Shotgun"], 1);
        inventory.AddItem(items["Bandage"], 10);
        inventory.AddItem(items["Chainmail"], 1);
        inventory.AddItem(items["Scarf"], 1);
        
    }

    private void Update()
    {
        //if the inventory button was pressed and the textbox isn't up open the inventory
        if (openInv.action.WasPressedThisFrame())
        {
            if (textboxCanvas.transform.childCount == 0)
                OpenOrCloseInventory();
        }
        //if the close button was pressed close the inventory
        if (close.action.WasPressedThisFrame())
        {
            //Hides the inventory GUI
            inventoryGUI.SetActive(false);
        }
        //If the interact button was pressed, the wait is over 2 and i've interacted with an item close the textbox
        if (interact.action.WasPressedThisFrame())
        {
            if (wait > 2 && interacting)
            {
                //Destroy all children in the textboxcanvas to remove the textbox
                for (int i = textboxCanvas.transform.childCount - 1; i >= 0; --i)
                    Destroy(textboxCanvas.transform.GetChild(i).gameObject);

                //resets "interacting" and reopenes the inventory
                interacting = false;
                OpenOrCloseInventory();
            }
        }
        //The wait prevents you from closing the textbox the same frame as you use the item
        wait++;
    }

    /// <summary>
    /// Refresh inventoru GUI and make it visible
    /// </summary>
    public void OpenOrCloseInventory()
    {
        //If the inventory is active, close it but making the GUI inactive
        if (inventoryGUI.activeSelf)
        {
            inventoryGUI.SetActive(false);
        }
        //Else make the GUI active, set the first item as selected and refresh inventory
        else
        {
            inventoryGUI.SetActive(true);
            if (inventoryGUI.transform.GetChild(12).gameObject.activeSelf)
                eventSystem.SetSelectedGameObject(inventoryGUI.transform.GetChild(12).GetChild(0).gameObject);

            RefreshInventory();
        }
    }

    /// <summary>
    /// Refreshes inventory GUI
    /// </summary>
    public void RefreshInventory()
    {
        //Resets all textboxes in the inventory GUI
        foreach (var item in invList)
        {
            item.GetComponent<TextMeshProUGUI>().text = "-";
        }
        //Sorts the inventory in alphabetical order
        inventory.Stacks.Sort((p1, p2) => { return string.Compare(p1.Item.name, p2.Item.name); });

        //Sets the GUI hp, food, weapon, equipment and all ammo to their 
        uiHp.text = $"Hp: {GlobalVariables.Hp}/{GlobalVariables.MaxHp}";
        uiFood.text = $"Food: n/a";
        if (GlobalVariables.EquippedWeapon != null)
            uiWeapon.text = $"Weapon: {GlobalVariables.EquippedWeapon.name}";
    
        if (GlobalVariables.EquippedEquipment != null)
            uiEquipment.text = $"Equipment: {GlobalVariables.EquippedEquipment.name}";

        uiLightAmmo.text = $":{GlobalVariables.LightAmmo}";
        uiMediumAmmo.text = $":{GlobalVariables.MediumAmmo}";
        uiShotgunAmmo.text = $":{GlobalVariables.ShotgunAmmo}";

        //Reads the inventory and puts it in the corresponding positions in the inventory GUI
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
            //Set interacting to false since you aren't interacting with an item in the inventory when u open it
            interacting = false;
        }
    }

    /// <summary>
    /// Selects an item from the inventory
    /// </summary>
    /// <param name="button">The slot in the inventory to be selected</param>
    public void ItemSelected(int button)
    {
        //Checks that you didn't press on an empty inventory slot
        if (inventory.Stacks.Count > button)
        {
            //If you're in the chest move the item to the chest
            if (chestGUI.activeSelf)
            {
                //Using QI_Chest moves the selected item to the opened chest then refreshes both the inventory and chest GUI
                QI_Chest.Transaction(chestVendor, playerChestVendor, inventory.Stacks[button].Item, 1);
                RefreshInventory();
                InteractWithChest.RefreshChest();
            }
            //If you're not in a chest go down to the use/info/throw row and set "selectedItem" to the selected item
            else
            {
                eventSystem.SetSelectedGameObject(inventoryGUI.transform.GetChild(9).GetChild(0).gameObject);
                selectedItem = button;
            }
        }
    }

    public void ItemUse()
    {
        ItemInteracted(inventory.Stacks[selectedItem].Item.useMessage);
        if (inventory.Stacks[selectedItem].Item.GetType().ToString() == "QI_Healing")
        {
            GlobalVariables.Hp = Mathf.Clamp(GlobalVariables.Hp + (inventory.Stacks[selectedItem].Item as QI_Healing).healingAmount, 0, GlobalVariables.MaxHp);
        }
        else if (inventory.Stacks[selectedItem].Item.GetType().ToString() == "QI_Weapons")
        {
            if (GlobalVariables.EquippedWeapon != null)
            {
                inventory.AddItem(GlobalVariables.EquippedWeapon, 1);
            }

            GlobalVariables.EquippedWeapon = (inventory.Stacks[selectedItem].Item as QI_Weapons);
        }
        else if (inventory.Stacks[selectedItem].Item.GetType().ToString() == "QI_Equipment")
        {
            if (GlobalVariables.EquippedEquipment != null)
            {
                inventory.AddItem(GlobalVariables.EquippedEquipment, 1);
            }
            GlobalVariables.EquippedEquipment = (inventory.Stacks[selectedItem].Item as QI_Equipment);
        }
        inventory.RemoveItem(inventory.Stacks[selectedItem].Item.Name, 1);
    }


    public void ItemInfo()
    {
        ItemInteracted(inventory.Stacks[selectedItem].Item.Description);
    }

    public void ItemThrow()
    {
        ItemInteracted($"{inventory.Stacks[selectedItem].Item.Name} was thrown away");

        inventory.RemoveItem(inventory.Stacks[selectedItem].Item.Name, 1);
    }

    public void ItemInteracted(string text)
    {
        GameObject storyText = Instantiate(textboxPrefab, textboxCanvas.transform.TransformPoint(0, 384.5f, 0), Quaternion.identity, textboxCanvas.transform);
        storyText.GetComponentInChildren<TextMeshProUGUI>().text = text;
        storyText.transform.SetParent(textboxCanvas.transform, false);

        eventSystem.SetSelectedGameObject(inventoryGUI.transform.GetChild(12).GetChild(0).gameObject);
        inventoryGUI.SetActive(false);
        wait = 0;
        interacting = true;
    }
}
