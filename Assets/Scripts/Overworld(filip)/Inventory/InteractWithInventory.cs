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
    [SerializeField] private InputActionProperty closeInv;
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

    //
    private int selectedItem = 0;
    private int wait = 0;
    private bool interacting = false;

    private void Awake()
    {
        if (GlobalVariables.PlayerInventory != null)
            inventory.Stacks = GlobalVariables.PlayerInventory;
        else
            inventory = GetComponent<QI_Inventory>();

        playerChestVendor = GetComponent<QI_Chest>();
        InteractWithChest = GetComponent<InteractWithChest>();
    }
    // Start is called before the first frame update
    void Start()
    {
        items = itemDatabase.Getdictionary();
        inventory.AddItem(items["Shotgun"], 1);
        inventory.AddItem(items["Bandage"], 10);
        inventory.AddItem(items["Chainmail"], 1);
        inventory.AddItem(items["Scarf"], 1);
        
    }

    private void Update()
    {
        if (openInv.action.WasPressedThisFrame())
        {
            if (textboxCanvas.transform.childCount == 0)
                OpenInv();
        }

        if (closeInv.action.WasPressedThisFrame())
        {
            inventoryGUI.SetActive(false);
        }

        if (interact.action.WasPressedThisFrame())
        {
            if (wait > 2 && interacting)
            {
                int childCount = textboxCanvas.transform.childCount;
                for (int i = childCount - 1; i >= 0; --i)
                {
                    Destroy(textboxCanvas.transform.GetChild(i).gameObject);
                }
                RefreshInventory();
                inventoryGUI.SetActive(true);
                interacting = false;
            }
        }
        wait++;
    }

    public void OpenInv()
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
            RefreshInventory();
        }
    }
    public void RefreshInventory()
    {
        foreach (var item in invList)
        {
            item.GetComponent<TextMeshProUGUI>().text = "-";
        }
        inventory.Stacks.Sort((p1, p2) => { return string.Compare(p1.Item.name, p2.Item.name); });

        uiHp.text = $"Hp: {GlobalVariables.Hp.ToString()}/{GlobalVariables.MaxHp}";
        uiFood.text = $"Food: n/a";
        if (GlobalVariables.EquippedWeapon != null)
            uiWeapon.text = $"Weapon: {GlobalVariables.EquippedWeapon.name}";
    
        if (GlobalVariables.EquippedEquipment != null)
            uiEquipment.text = $"Equipment: {GlobalVariables.EquippedEquipment.name}";

        uiLightAmmo.text = $":{GlobalVariables.LightAmmo}";
        uiMediumAmmo.text = $":{GlobalVariables.MediumAmmo}";
        uiShotgunAmmo.text = $":{GlobalVariables.ShotgunAmmo}";

        for (int i = 0; i < inventory.Stacks.Count; i++)
        {
            GameObject itemRefresh = invList[i];

            if (itemDatabase.GetItem(inventory.Stacks[i].Item.Name).MaxStack != 1)
            {
                itemRefresh.GetComponent<TextMeshProUGUI>().text = $"- {inventory.Stacks[i].Item.Name} x{inventory.Stacks[i].Amount}";
            }
            else
            {
                itemRefresh.GetComponent<TextMeshProUGUI>().text = $"- {inventory.Stacks[i].Item.Name}";
            }
            interacting = false;
        }
    }

    public void ItemInteraction(int button)
    {
        if (inventory.Stacks.Count > button)
        {
            if (chestGUI.activeSelf)
            {
                QI_Chest.Transaction(chestVendor, playerChestVendor, inventory.Stacks[button].Item, 1);
                RefreshInventory();
                InteractWithChest.RefreshChest();
            }
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
