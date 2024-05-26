using QuantumTek.QuantumInventory;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Quantum Inventory/Weapon", fileName = "New Item")]

public class QI_Weapons : QI_ItemData
{
    public int weaponDamage;
    public int weaponType;
    public int weaponFireRate;
    public int weaponSpread;
    public int weaponMaxAmmo;
}
