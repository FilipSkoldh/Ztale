using QuantumTek.QuantumInventory;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GlobalVariables
{ 
    public static QI_Inventory PlayerInventory { get; set; }
    public static int MaxHp { get; set; }
    public static int Hp { get; set; }
    public static QI_Weapons EquippedWeapon { get; set; }
    public static QI_Equipment EquippedEquipment { get; set; }
}