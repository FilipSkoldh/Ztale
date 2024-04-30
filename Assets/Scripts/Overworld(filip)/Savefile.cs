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
    public string equippedWeapon;
    public string equippedEquipment;

    public Dictionary<string, int>[] inventories = new Dictionary<string, int>[2];

}
