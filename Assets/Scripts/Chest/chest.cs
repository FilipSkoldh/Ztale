using QuantumTek.QuantumInventory;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class chest : MonoBehaviour
{
    [SerializeField] private GameObject invGameobj;
    [SerializeField] private EventSystem eventSystem;
    [SerializeField] private List<GameObject> invList;
    public QI_Inventory inventory;
    public QI_ItemDatabase itemDatabase;
    private Dictionary<string, QI_ItemData> items = new Dictionary<string, QI_ItemData>();


}
