using QuantumTek.QuantumInventory;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveAndLoad : MonoBehaviour
{
    [SerializeField] private QI_ItemDatabase itemDatabase;
    private Dictionary<string, QI_ItemData> items = new();

    public QI_Inventory[] chests = new QI_Inventory[1];
    public QI_FilledInventory[] FilledInventories = new QI_FilledInventory[2];
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
        
        GlobalVariables.PlayerInventory = FilledInventories[0].Stacks;
        for (int i = 1; i < FilledInventories.Length; i++)
        {
            chests[i-1].Stacks = FilledInventories[i].Stacks;
        }
        
    }

    public void NewSave()
    {
        
    }
}