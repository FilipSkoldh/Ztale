using QuantumTek.QuantumInventory;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GlobalVariables
{
    public static List<QI_ItemStack> PlayerInventory { get; set; }

    public static int MaxHp { get; set; }
    public static int Hp { get; set; }
    public static QI_Weapons EquippedWeapon { get; set; }
    public static int EquippedWeaponAmmo { get; set; }
    public static QI_Equipment EquippedEquipment { get; set; }
    public static int LightAmmo { get; set; }
    public static int MediumAmmo { get; set;}
    public static int ShotgunAmmo { get;set; }
    public static int Encounter { get; set ; }

}