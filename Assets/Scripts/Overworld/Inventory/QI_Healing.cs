using QuantumTek.QuantumInventory;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Quantum Inventory/HealingItem", fileName = "New Item")]

public class QI_Healing : QI_ItemData
{
    public int  healingAmount;
}