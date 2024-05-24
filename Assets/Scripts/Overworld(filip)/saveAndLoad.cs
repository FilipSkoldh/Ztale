using Newtonsoft.Json;
using QuantumTek.QuantumInventory;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;



public class SaveAndLoad : MonoBehaviour
{
    [SerializeField] private QI_ItemDatabase itemDatabase;
    private Animator playerAnimator;
    private Transform playerTransform;
    public QI_Inventory[] Inventories = new QI_Inventory[2];
    public QI_Chest[] transferChests = new QI_Chest[2];
    public Dictionary<string, QI_ItemData> items = new();

    private void Awake()
    {
        playerAnimator = GetComponent<Animator>();
        playerTransform = GetComponent<Transform>();
        items = itemDatabase.Getdictionary();
        LoadSave();
    }
    string savefilePath = $"{Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)}\\Documents\\My Games\\Ztale\\Saves";
    Savefile savefile = new();

    public void SaveScene()
    {
        GlobalVariables.Inventories = new List<List <QI_ItemStack>>();
        for (int i = 0; i < 2; i++)
        {
            GlobalVariables.Inventories.Add(Inventories[i].Stacks);
        }

        GlobalVariables.PlayerPosition = playerTransform.position;
        GlobalVariables.PlayerAnimatorX = playerAnimator.GetFloat("x");
        GlobalVariables.PlayerAnimatorY = playerAnimator.GetFloat("y");

    }

    public void Save(int saveLocation)
    {
        savefile.saveLocation = saveLocation;
        savefile.maxHp = GlobalVariables.MaxHp;
        savefile.hp = GlobalVariables.Hp;
        savefile.lightAmmo = GlobalVariables.LightAmmo;
        savefile.mediumAmmo = GlobalVariables.MediumAmmo;
        savefile.shoutgunAmmo = GlobalVariables.ShotgunAmmo;
        savefile.playerName = GlobalVariables.PlayerName;

        Debug.Log(GlobalVariables.PlayerName);
        
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
        
       
            File.WriteAllText($"{savefilePath}\\Save{GlobalVariables.Savefile}", json);
        
    }

    public void LoadSave()
    {
        if (GlobalVariables.LoadedSave)
        {
            playerTransform.position = GlobalVariables.PlayerPosition;
            playerAnimator.SetFloat("x", GlobalVariables.PlayerAnimatorX);
            playerAnimator.SetFloat("y", GlobalVariables.PlayerAnimatorY);

            for (int i = 0; i < 2; i++)
            {
                Inventories[i].Stacks = GlobalVariables.Inventories[i];
            }

            return;
        }

        string saveData;


        if (File.Exists($"{savefilePath}\\Save{GlobalVariables.Savefile}"))
        {
            saveData = File.ReadAllText($"{savefilePath}\\Save{GlobalVariables.Savefile}");
        }
        else 
        {
            saveData = File.ReadAllText(Path.Combine(Environment.CurrentDirectory, @"Assets\NewSaveData.json"));

        }


        savefile = JsonConvert.DeserializeObject<Savefile>(saveData);

        GlobalVariables.MaxHp = savefile.maxHp;
        GlobalVariables.Hp = savefile.hp;
        GlobalVariables.LightAmmo = savefile.lightAmmo;
        GlobalVariables.MediumAmmo = savefile.mediumAmmo;
        GlobalVariables.ShotgunAmmo = savefile.shoutgunAmmo;

        if (savefile.playerName != null)
            GlobalVariables.PlayerName = savefile.playerName;

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
        GlobalVariables.LoadedSave = true;

    }
}