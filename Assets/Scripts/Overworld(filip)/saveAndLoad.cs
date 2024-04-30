using QuantumTek.QuantumInventory;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;
using System.Linq;
using Unity.VisualScripting;



public class SaveAndLoad : MonoBehaviour
{
    [SerializeField] private QI_ItemDatabase itemDatabase;
    public QI_Inventory[] Inventories = new QI_Inventory[2];
    public QI_Chest[] transferChests = new QI_Chest[2];
    public Dictionary<string, QI_ItemData> items = new();

    private void Awake()
    {
        items = itemDatabase.Getdictionary();
        LoadSave();
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
        
        if (GlobalVariables.EquippedWeapon != null)
            savefile.equippedWeapon = GlobalVariables.EquippedWeapon.name;

        if (GlobalVariables.EquippedEquipment != null)
            savefile.equippedEquipment = GlobalVariables.EquippedEquipment.name;

        
        for (int i = 0; i < Inventories.Length; i++)
        {
            if (Inventories[i] == null)
                return;

            savefile.inventories[i] = new Dictionary<string, int>();
            for (int j = 0; j < Inventories[i].Stacks.Count; j++)
            {
                Debug.Log($"{Inventories[i].Stacks[j].Item.name} + {Inventories[i].Stacks[j].Amount} + {savefile.inventories[i].ToString()}");                
                savefile.inventories[i].Add(Inventories[i].Stacks[j].Item.name, Inventories[i].Stacks[j].Amount);
            }
        }
        Debug.Log(savefile.inventories[0].Count);
        string json = JsonConvert.SerializeObject(savefile);
        Debug.Log(json);
        

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

        if (savefile.equippedEquipment != null )
            GlobalVariables.EquippedEquipment = (items[savefile.equippedEquipment])as QI_Equipment;

        if (savefile.equippedWeapon != null )
            GlobalVariables.EquippedWeapon = (items[savefile.equippedWeapon] as QI_Weapons);


        for (int i = 0; i < savefile.inventories.Length; i++)
        {
            if (savefile.inventories[i] == null)
                return;

            Inventories[i].Stacks.Clear();
            for (int j = 0; j < savefile.inventories[i].Count; j++)
            {
                Debug.Log(savefile.inventories[i].Count);
                Debug.Log(j);
                Inventories[i].AddItem(items[savefile.inventories[i].ElementAt(j).Key], savefile.inventories[i][savefile.inventories[i].ElementAt(j).Key]);
            }
        }


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