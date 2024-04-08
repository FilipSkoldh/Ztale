using QuantumTek.QuantumInventory;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class interactWithChest : MonoBehaviour
{
    [SerializeField] private GameObject invGameobj;
    [SerializeField] private EventSystem eventSystem;
    [SerializeField] private List<GameObject> invList;
    [SerializeField] private saveAndLoad saveAndLoad;
    private QI_Inventory inventory;
    public QI_ItemDatabase itemDatabase;
    private Dictionary<string, QI_ItemData> items = new Dictionary<string, QI_ItemData>();

    public void Open_Chest(int inventoryNumber)
    {
        items = itemDatabase.Getdictionary();
        inventory = saveAndLoad.inventories[inventoryNumber];
        
        inventory.AddItem(items["Bandages"], 5);

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
}
