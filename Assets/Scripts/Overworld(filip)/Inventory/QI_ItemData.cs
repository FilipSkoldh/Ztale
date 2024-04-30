using UnityEngine;

namespace QuantumTek.QuantumInventory
{
    /// <summary>
    /// QI_ItemData stores data about an item.
    /// </summary>
    [CreateAssetMenu(menuName = "Quantum Inventory/Item", fileName = "New Item")]
    public class QI_ItemData : ScriptableObject
    {
        public string Name;
        public string Description;
        public string useMessage;
        public float Price;
        public int MaxStack;
    }
}