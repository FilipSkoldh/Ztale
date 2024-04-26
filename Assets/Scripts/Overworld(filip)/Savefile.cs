using QuantumTek.QuantumInventory;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class Savefile
{
    public int maxHp;
    public int hp;
    public int lightAmmo;
    public int mediumAmmo;
    public int shoutgunAmmo;
    public int saveLocation;
    public QI_Weapons equippedWeapon;
    public QI_Equipment equippedEquipment;
    public List<QI_ItemStack> player = new List<QI_ItemStack>();
    public List<QI_ItemStack> chest = new List<QI_ItemStack>();

    public List<List<QI_ItemStack>> Stacks = new();

}
