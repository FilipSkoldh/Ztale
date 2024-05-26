using QuantumTek.QuantumInventory;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class Savefile
{
    /// <summary>
    /// The players maxHP
    /// </summary>
    public int maxHp;

    /// <summary>
    /// The players current HP
    /// </summary>
    public int hp;

    /// <summary>
    /// The current amount of lightAmmo
    /// </summary>
    public int lightAmmo;

    /// <summary>
    /// The current amount of mediumAmmo
    /// </summary>
    public int mediumAmmo;

    /// <summary>
    /// The current amount of shotgumAmmo
    /// </summary>
    public int shoutgunAmmo;

    /// <summary>
    /// The current equipped Weapon
    /// </summary>
    public string equippedWeapon;

    /// <summary>
    /// The current equipped Equipment
    /// </summary>
    public string equippedEquipment;

    /// <summary>
    /// The players name
    /// </summary>
    public string playerName;

    /// <summary>
    /// All inventories
    /// </summary>
    public Dictionary<string, int>[] inventories = new Dictionary<string, int>[2];

    /// <summary>
    /// Players position
    /// </summary>
    public Vector3 playerPosition;

    /// <summary>
    /// animators X value
    /// </summary>
    public float animatorX;

    /// <summary>
    /// animators Y value
    /// </summary>
    public float animatorY;
}
