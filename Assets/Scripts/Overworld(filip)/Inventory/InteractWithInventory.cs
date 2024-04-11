using QuantumTek.QuantumInventory;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class InteractWithInventory : MonoBehaviour
{
    [SerializeField] private InputActionProperty openInv;
    [SerializeField] private InputActionProperty closeInv;
    [SerializeField] private InputActionProperty interact;
    [SerializeField] private GameObject inventoryGUI;
    [SerializeField] private EventSystem eventSystem;
    [SerializeField] private List<GameObject> invList;
    [SerializeField] private GameObject chestGUI;
    [SerializeField] private InteractWithChest InteractWithChest;
    [SerializeField] private QI_Chest chestVendor;
    [SerializeField] private QI_Chest playerChestVendor;
    [SerializeField] private GameObject textPrefab;
    [SerializeField] private Canvas canvas;
    public QI_ItemStack equipped;
    public QI_Inventory inventory;
    public QI_ItemDatabase itemDatabase;
    private Dictionary<string, QI_ItemData> items = new Dictionary<string, QI_ItemData>();
    private int selectedItem = 0;
    private bool itemInteracted = false;


    // Start is called before the first frame update
    void Start()
    {
        items = itemDatabase.Getdictionary();
        inventory.AddItem(items["Shotgun"], 1);
        inventory.AddItem(items["Bandages"], 5);
    }

    private void Update()
    {
        if (openInv.action.WasPressedThisFrame())
        {
            OpenInv();
        }
        if (closeInv.action.WasPressedThisFrame())
        {
            inventoryGUI.SetActive(false);
        }
        if (interact.action.WasPressedThisFrame())
        {
            if (itemInteracted)
            {
                int childCount = canvas.transform.childCount;
                for (int i = childCount - 1; i >= 0; --i)
                {
                    Destroy(canvas.transform.GetChild(i).gameObject);
                }
                inventoryGUI.SetActive(true);
            }
        }
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

    public void ItemInteraction(int button)
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


    public void ItemInfo()
    {
        GameObject storyText = Instantiate(textPrefab, canvas.transform.TransformPoint(0, 384.5f, 0), Quaternion.identity, canvas.transform);
        storyText.GetComponentInChildren<TextMeshProUGUI>().text = inventory.Stacks[selectedItem].Item.Description;
        storyText.transform.SetParent(canvas.transform, false);
        inventoryGUI.SetActive(false);
    }

    public void ItemThrow()
    {
        GameObject storyText = Instantiate(textPrefab, canvas.transform.TransformPoint(0, 384.5f, 0), Quaternion.identity, canvas.transform);
        storyText.GetComponentInChildren<TextMeshProUGUI>().text = $"{inventory.Stacks[selectedItem].Item.name} was thrown away";
        storyText.transform.SetParent(canvas.transform, false);
        inventory.RemoveItem(inventory.Stacks[selectedItem].Item.name, 1);
        inventoryGUI.SetActive(false);
    }
}
