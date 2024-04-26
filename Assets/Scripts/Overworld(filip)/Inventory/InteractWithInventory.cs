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

    //All GUI
    [SerializeField] private GameObject inventoryGUI;
    [SerializeField] private GameObject chestGUI;
    [SerializeField] private GameObject textboxPrefab;
    [SerializeField] private Canvas textboxCanvas;
    [SerializeField] private TextMeshProUGUI uiHp;
    [SerializeField] private TextMeshProUGUI uiFood;
    [SerializeField] private TextMeshProUGUI uiWeapon;
    [SerializeField] private TextMeshProUGUI uiEquipment;
    [SerializeField] private TextMeshProUGUI uiLightAmmo;
    [SerializeField] private TextMeshProUGUI uiMediumAmmo;
    [SerializeField] private TextMeshProUGUI uiShotgunAmmo;

    //The inventory slots and the EventSystem
    [SerializeField] private List<GameObject> invList;
    [SerializeField] private EventSystem eventSystem;

    //The players inventory
    private QI_Inventory inventory;

    //the database and dictionary with all item info
    [SerializeField] private QI_ItemDatabase itemDatabase;

    //Other scripts
    [SerializeField] private QI_Chest chestVendor;
    private QI_Chest playerChestVendor;
    private InteractWithChest InteractWithChest;
    private SaveAndLoad saveAndLoad;

    //variables for inventory
    private QI_ItemData selectedItem;
    private int wait = 0;
    private bool interacting = false;

    private void Awake()
    {
        saveAndLoad = GetComponent<SaveAndLoad>();
        saveAndLoad.LoadSave();
        //Finds components on the player
        inventory = GetComponent<QI_Inventory>();
        playerChestVendor = GetComponent<QI_Chest>();
        InteractWithChest = GetComponent<InteractWithChest>();

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
            //If there's a inventory saved globally load it else load the one on the player
            if (GlobalVariables.PlayerInventory != null)
                inventory.Stacks = GlobalVariables.PlayerInventory;
            else
                inventory = GetComponent<QI_Inventory>();

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
        }


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

        //Set interacting to false since you aren't interacting with an item in the inventory when u open it
        interacting = false;
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

                Debug.Log(button);
                Debug.Log($"  {chestVendor}   {playerChestVendor}  {inventory.Stacks[button].Item}");
                QI_Chest.Transaction(chestVendor, playerChestVendor, inventory.Stacks[button].Item, 1);
                RefreshInventory();
                InteractWithChest.RefreshChest();
            }
            //If you're not in a chest go down to the use/info/throw row and set "selectedItem" to the selected item
            else
            {
                eventSystem.SetSelectedGameObject(inventoryGUI.transform.GetChild(9).GetChild(0).gameObject);
                selectedItem = inventory.Stacks[button].Item;
            }
        }
    }

    /// <summary>
    /// Uses the selected item in it's intended way
    /// </summary>
    public void ItemUse()
    {
        //removes the selected item from the inventory
        inventory.RemoveItem(selectedItem.Name, 1);

        //Gets the items use message and displays it in the textbox
        ItemInteracted(selectedItem.useMessage);

        //Checks what item type it is
        //Healing heals the player
        if (selectedItem.GetType().ToString() == "QI_Healing")
        {
            //heales the amount it is supposed to
            GlobalVariables.Hp = Mathf.Clamp(GlobalVariables.Hp + (selectedItem as QI_Healing).healingAmount, 0, GlobalVariables.MaxHp);
        }
        //Weapons are equipped
        else if (selectedItem.GetType().ToString() == "QI_Weapons")
        {
            //If you had a weapon equipped already it's added back to the inventory
            if (GlobalVariables.EquippedWeapon != null)
            {
                inventory.AddItem(GlobalVariables.EquippedWeapon, 1);
            }
            //Changing the equipped weapon to the weapon you selected
            GlobalVariables.EquippedWeapon = (selectedItem as QI_Weapons);
        }
        //Equipment are equipped
        else if (selectedItem.GetType().ToString() == "QI_Equipment")
        {
            //If you hade equipment equipped already it's added back to the inventory
            if (GlobalVariables.EquippedEquipment != null)
            {
                inventory.AddItem(GlobalVariables.EquippedEquipment, 1);
            }
            //Changing the equipped equipment to the equipment you selected
            GlobalVariables.EquippedEquipment = (selectedItem as QI_Equipment);
        }
    }

    /// <summary>
    /// Displays the selected items description
    /// </summary>
    public void ItemInfo()
    {
        ItemInteracted(selectedItem.Description);
    }

    /// <summary>
    /// Deletes the selected item
    /// </summary>
    public void ItemThrow()
    {
        ItemInteracted($"{selectedItem.Name} was thrown away");
        inventory.RemoveItem(selectedItem.Name, 1);
    }

    /// <summary>
    /// Displays a single line in the textbox
    /// </summary>
    /// <param name="text">The text that is going to be displayed</param>
    public void ItemInteracted(string text)
    {
        //spawn the textbox prefab
        GameObject storyText = Instantiate(textboxPrefab, textboxCanvas.transform.TransformPoint(0, 384.5f, 0), Quaternion.identity, textboxCanvas.transform);
        //text the TextmeshproUGUI to "text"
        storyText.GetComponentInChildren<TextMeshProUGUI>().text = text;

        //deactivates the inventory GUI, resets "wait" so the textbox isn't closed on the same frame
        inventoryGUI.SetActive(false);
        wait = 0;
        interacting = true;
    }
}
