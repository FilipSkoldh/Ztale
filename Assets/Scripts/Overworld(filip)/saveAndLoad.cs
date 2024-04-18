using QuantumTek.QuantumInventory;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveAndLoad : MonoBehaviour
{ 

    public QI_Inventory[] chests = new QI_Inventory[1];
    public QI_Chest[] transferChests = new QI_Chest[2];



    
    public void Save()
    {

    }
    public void LoadSave()
    {
        GlobalVariables.MaxHp = 100;
        GlobalVariables.Hp = 10;
        GlobalVariables.LightAmmo = 20;
        GlobalVariables.MediumAmmo = 15;
        GlobalVariables.ShotgunAmmo = 10;
    }


}
