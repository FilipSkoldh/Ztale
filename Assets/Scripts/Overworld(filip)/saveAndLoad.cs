using Ink.Runtime;
using Newtonsoft.Json;
using QuantumTek.QuantumInventory;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;



public class SaveAndLoad : MonoBehaviour
{
    //declaring variables

    //item database with all itemdata
    [SerializeField] private QI_ItemDatabase itemDatabase;
    [SerializeField] private TextAsset NewSave;
    //players animator and transform
    private Animator playerAnimator;
    private Transform playerTransform;

    //all inventories and chest scripts
    public QI_Inventory[] Inventories = new QI_Inventory[2];
    public QI_Chest[] transferChests = new QI_Chest[2];

    //Dictionary with item names and itemdata
    public Dictionary<string, QI_ItemData> items = new();


    private void Awake()
    {
        //get components from the player
        playerAnimator = GetComponent<Animator>();
        playerTransform = GetComponent<Transform>();

        //fill the dictionary with entire database
        items = itemDatabase.Getdictionary();
        LoadSave();
    }



    /// <summary>
    /// the path to where savefiles are stored
    /// </summary>
    string savefilePath = $"{Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)}\\Documents\\My Games\\Ztale\\Saves";

    //create a Savefile variable to load or save data
    Savefile savefile = new();


    /// <summary>
    /// Starts an encounter as well as saving imortant data
    /// </summary>
    /// <param name="encounter">The encounter to start</param>
    public void StartEncounter(int encounter)
    {
        //resets the "GlobalVariables.inventories"
        GlobalVariables.Inventories = new List<List <QI_ItemStack>>();

        //saves all inventories
        for (int i = 0; i < 2; i++)
        {
            GlobalVariables.Inventories.Add(Inventories[i].Stacks);
        }

        //if the equipped weapon is meele set EquippedWeaponAmmo to -1
        if(GlobalVariables.EquippedWeapon != null)
        if (GlobalVariables.EquippedWeapon.weaponMaxAmmo < 0)
            GlobalVariables.EquippedWeaponAmmo = -1;

        //saving rest of data to "GlobalVariables"
        GlobalVariables.Encounter = encounter;
        GlobalVariables.PlayerPosition = playerTransform.position;
        GlobalVariables.PlayerAnimatorX = playerAnimator.GetFloat("x");
        GlobalVariables.PlayerAnimatorY = playerAnimator.GetFloat("y");
        SceneManager.LoadScene("Battle");
    }


    /// <summary>
    /// Saves the current gamedata to the savefile
    /// </summary>
    /// <param name="saveLocation">The Location you saved at</param>
    public void Save(int saveLocation)
    {
        //saves current data from "GlobalVariables" to "savefile"
        savefile.playerPosition = playerTransform.position;
        savefile.animatorX = playerAnimator.GetFloat("x");
        savefile.animatorY = playerAnimator.GetFloat("y");
        savefile.maxHp = GlobalVariables.MaxHp;
        savefile.hp = GlobalVariables.Hp;
        savefile.lightAmmo = GlobalVariables.LightAmmo;
        savefile.mediumAmmo = GlobalVariables.MediumAmmo;
        savefile.shoutgunAmmo = GlobalVariables.ShotgunAmmo;
        savefile.playerName = GlobalVariables.PlayerName;

        //only save "EquippedWeapon" and "EquippedEquipment" if they aren't null
        if (GlobalVariables.EquippedWeapon != null)
            savefile.equippedWeapon = GlobalVariables.EquippedWeapon.name;

        if (GlobalVariables.EquippedEquipment != null)
            savefile.equippedEquipment = GlobalVariables.EquippedEquipment.name;

        //saving all inventories
        for (int i = 0; i < Inventories.Length; i++)
        {
            //return if the inventory it's currently trying to save is null
            if (Inventories[i] == null)
                return;

            //creates the inventory dictionary in the list "savefile.inventories" which hold amount of item and which item
            savefile.inventories[i] = new Dictionary<string, int>();
            //for every stackin the inventory add the itemname and amount as an entry in the dictionary
            for (int j = 0; j < Inventories[i].Stacks.Count; j++)
            {       
                savefile.inventories[i].Add(Inventories[i].Stacks[j].Item.name, Inventories[i].Stacks[j].Amount);
            }
        }

        //serializes "savefile"
        string json = JsonConvert.SerializeObject(savefile);
        
        //writes the saved data to the savefile
        File.WriteAllText($"{savefilePath}\\Save{GlobalVariables.Savefile}", json);
        
    }


    /// <summary>
    /// Load the selected savefile
    /// </summary>
    public void LoadSave()
    {

        //if the savefile is already loaded instad load the data from "GlobalVariables"
        if (GlobalVariables.LoadedSave && !GlobalVariables.Playerdead)
        {
            //loads position and which direction to face
            playerTransform.position = GlobalVariables.PlayerPosition;
            playerAnimator.SetFloat("x", GlobalVariables.PlayerAnimatorX);
            playerAnimator.SetFloat("y", GlobalVariables.PlayerAnimatorY);

            //loads all inventories
            for (int i = 0; i < 2; i++)
            {
                Inventories[i].Stacks = GlobalVariables.Inventories[i];
            }
            return;
        }

        //the Json data to deserialze
        string saveData;
        //if the saveslot selected has a savefile load it otherwise load a new savefile
        if (File.Exists($"{savefilePath}\\Save{GlobalVariables.Savefile}"))
        {
            saveData = File.ReadAllText($"{savefilePath}\\Save{GlobalVariables.Savefile}");
        }
        else
        {
            saveData = NewSave.text;
        }
        savefile = JsonConvert.DeserializeObject<Savefile>(saveData);


        playerTransform.position = savefile.playerPosition;
        playerAnimator.SetFloat("x", savefile.animatorX);
        playerAnimator.SetFloat("y", savefile.animatorY);

        //load data from "savefile" into "GlobalVariables"
        GlobalVariables.MaxHp = savefile.maxHp;
        GlobalVariables.Hp = savefile.hp;
        GlobalVariables.LightAmmo = savefile.lightAmmo;
        GlobalVariables.MediumAmmo = savefile.mediumAmmo;
        GlobalVariables.ShotgunAmmo = savefile.shoutgunAmmo;

        //only load name, EquippedEquipment and EquippedWeapon if they aren't null
        if (savefile.playerName != null)
            GlobalVariables.PlayerName = savefile.playerName;

        if (savefile.equippedEquipment != null )
            GlobalVariables.EquippedEquipment = (items[savefile.equippedEquipment])as QI_Equipment;

        if (savefile.equippedWeapon != null )
            GlobalVariables.EquippedWeapon = (items[savefile.equippedWeapon] as QI_Weapons);

        //loads all inventories from "savefile.inventories" into "Inventories"
        for (int i = 0; i < savefile.inventories.Length; i++)
        {
            //only load if the inventory isn't null
            if (savefile.inventories[i] == null)
                return;

            //clear the inventory to be loaded into
            Inventories[i].Stacks.Clear();
            //for every itemstack in the "savefiles.inventories[i]" dictinary add it's item and amount to "Inventory"
            for (int j = 0; j < savefile.inventories[i].Count; j++)
            {
                //gets item name and amount from "savefile.inventories[i] dictorionary and uses the name in "items" dictonary to get the itemdata
                Inventories[i].AddItem(items[savefile.inventories[i].ElementAt(j).Key], savefile.inventories[i][savefile.inventories[i].ElementAt(j).Key]);
            }
        }
        GlobalVariables.LoadedSave = true;
        GlobalVariables.Playerdead = false;
    }

    /// <summary>
    /// Gives the player some ammo
    /// </summary>
    public void GiveAmmo()
    {
        GlobalVariables.ShotgunAmmo = 10;
        GlobalVariables.LightAmmo = 20;
    }
}