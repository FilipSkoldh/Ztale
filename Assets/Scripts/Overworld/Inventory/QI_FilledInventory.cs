using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QuantumTek.QuantumInventory
{
    [CreateAssetMenu(menuName = "Quantum Inventory/FilledInventory", fileName = "New Inventory")]
    public class QI_FilledInventory : ScriptableObject
    {
        public List<QI_ItemStack> Stacks = new();
    }
}