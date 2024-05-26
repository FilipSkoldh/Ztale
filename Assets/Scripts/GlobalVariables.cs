using QuantumTek.QuantumInventory;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public static class GlobalVariables
{
    /// <summary>
    /// All inventories
    /// </summary>
    public static List<List <QI_ItemStack>> Inventories { get; set; }

    /// <summary>
    /// If a savefile is loaded
    /// </summary>
    public static bool LoadedSave {  get; set; }

    /// <summary>
    /// Players position when going into other scenes
    /// </summary>
    public static Vector2 PlayerPosition { get; set; }

    /// <summary>
    /// Players animators X when going into other scenes
    /// </summary>
    public static float PlayerAnimatorX { get; set; }

    /// <summary>
    /// Players animators Y when going into other scenes
    /// </summary>
    public static float PlayerAnimatorY { get; set; }

    /// <summary>
    /// Players maxHP
    /// </summary>
    public static int MaxHp { get; set; }

    /// <summary>
    /// Players current HP
    /// </summary>
    public static int Hp { get; set; }

    /// <summary>
    /// Current equipped weapon
    /// </summary>
    public static QI_Weapons EquippedWeapon { get; set; }

    /// <summary>
    /// Current equipped Equipment
    /// </summary>
    public static QI_Equipment EquippedEquipment { get; set; }

    /// <summary>
    /// Current LightAmmo amount
    /// </summary>
    public static int LightAmmo { get; set; }


    /// <summary>
    /// Current MeduimAmmo amount
    /// </summary>
    public static int MediumAmmo { get; set;}

    /// <summary>
    /// Current ShougunAmmo amount
    /// </summary>
    public static int ShotgunAmmo { get;set; }

    /// <summary>
    /// Which encounter to load when loading "battle" scene
    /// </summary>
    public static int Encounter { get; set ; }

    /// <summary>
    /// Which savefile is selected
    /// </summary>
    public static int Savefile { get; set; }

    /// <summary>
    /// The players name
    /// </summary>
    public static string PlayerName { get; set; }

    /// <summary>
    /// AmmoCount of Equipped Weapon
    /// </summary>
    public static int EquippedWeaponAmmo { get; set; }

    /// <summary>
    /// Did the player die in the encounter
    /// </summary>
    public static bool Playerdead { get; set; }

    /// <summary>
    /// is the zombie dead
    /// </summary>
    public static bool ZombieDead { get; set; }
}