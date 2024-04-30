using QuantumTek.QuantumInventory;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;



public class SaveAndLoad : MonoBehaviour
{
    [SerializeField] private QI_ItemDatabase itemDatabase;
    public QI_Inventory[] Inventories = new QI_Inventory[2];
    public QI_Chest[] transferChests = new QI_Chest[2];
    public Dictionary<string, QI_ItemData> items = new();

    private void Awake()
    {
        items = itemDatabase.Getdictionary();
    }

    //string savefilePath = "C:\\Users\\22skofil\\Documents\\My Games\\Ztale\\Saves\\PlayerData.json";
    string savefilePath = "C:\\temp\\PlayerData.json";
    Savefile savefile = new();


    public void Save(int saveLocation)
    {
        savefile.saveLocation = saveLocation;
        savefile.maxHp = GlobalVariables.MaxHp;
        savefile.hp = GlobalVariables.Hp;
        savefile.lightAmmo = GlobalVariables.LightAmmo;
        savefile.mediumAmmo = GlobalVariables.MediumAmmo;
        savefile.shoutgunAmmo = GlobalVariables.ShotgunAmmo;
        savefile.equippedEquipment = GlobalVariables.EquippedEquipment;
        savefile.equippedWeapon = GlobalVariables.EquippedWeapon;

        savefile.player = Inventories[0].Stacks;
        savefile.chest = Inventories[1].Stacks;

        for (int i = 0; i < savefile.Stacks.Count; i++)
        {
            savefile.Stacks[i] = Inventories[i].Stacks;
        }

        string json = JsonConvert.SerializeObject(savefile);
        

        File.WriteAllText(savefilePath, json);
    }

    public void LoadSave()
    {
        string saveData = File.ReadAllText(savefilePath);
        Debug.Log(saveData);
        savefile = JsonConvert.DeserializeObject<Savefile>(saveData);

        GlobalVariables.MaxHp = savefile.maxHp;
        GlobalVariables.Hp = savefile.hp;
        GlobalVariables.LightAmmo = savefile.lightAmmo;
        GlobalVariables.MediumAmmo = savefile.mediumAmmo;
        GlobalVariables.ShotgunAmmo = savefile.shoutgunAmmo;
        GlobalVariables.EquippedEquipment = savefile.equippedEquipment;
        GlobalVariables.EquippedWeapon = savefile.equippedWeapon;

        for (int i = 0; i < savefile.Stacks.Count; i++)
        {
            if (savefile.Stacks[i] != null)
                Inventories[i].Stacks = savefile.Stacks[i];
          

            for (int j = 0; j < savefile.Stacks[i].Count; j++)
            {
                if (savefile.Stacks[i][j].Item.name == null)
                {
                    savefile.Stacks[i].RemoveAt(j);
                }
            }
        }

        GlobalVariables.PlayerInventory = Inventories[0].Stacks;

    }

    public void FillInventories()
    {
        for (int i = 0; i < Inventories.Length; i++)
        {
            Inventories[i].Stacks.Clear();
        }
        Inventories[0].AddItem(items["Shotgun"], 1);
        Inventories[0].AddItem(items["Scarf"], 1);
        Inventories[0].AddItem(items["Chainmail"], 1);
        Inventories[0].AddItem(items["Bandage"], 10);
        Inventories[1].AddItem(items["Bandage"], 10);
        Inventories[1].AddItem(items["Pistol"], 1);

    }

    public void NewSave()
    {

    }
}