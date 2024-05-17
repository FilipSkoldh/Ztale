using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GUIDisplay : MonoBehaviour
{
    private TextMeshProUGUI hpText;
    private TextMeshProUGUI weaponText;
    private TextMeshProUGUI equipmentText;
    private TextMeshProUGUI LightAmmoText;
    private TextMeshProUGUI ShotgunAmmoText;
    private TextMeshProUGUI MediumAmmoText;

    private void Awake()
    {
        hpText = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        weaponText = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        equipmentText = transform.GetChild(2).GetComponent<TextMeshProUGUI>();
        LightAmmoText = transform.GetChild(3).GetComponent<TextMeshProUGUI>();
        ShotgunAmmoText = transform.GetChild(4).GetComponent<TextMeshProUGUI>();
        MediumAmmoText = transform.GetChild(5).GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        hpText.text = $" Hp: {GlobalVariables.Hp} / {GlobalVariables.MaxHp}";
        if (GlobalVariables.EquippedWeapon == null)
            weaponText.text = " Weapon:";
        else
        {
            if (GlobalVariables.EquippedWeapon.weaponType == 3)
                weaponText.text = $" Weapon: {GlobalVariables.EquippedWeapon.name}";
            else
                weaponText.text = $" Weapon: {GlobalVariables.EquippedWeapon.name} {GlobalVariables.EquippedWeaponAmmo} / {GlobalVariables.EquippedWeapon.weaponMaxAmmo}";
        }
        if (GlobalVariables.EquippedEquipment == null)
            equipmentText.text = " Equipment:";
        else
            equipmentText.text = $" Equipment: {GlobalVariables.EquippedEquipment.name}";
        LightAmmoText.text = $" : {GlobalVariables.LightAmmo}";
        ShotgunAmmoText.text = $" : {GlobalVariables.ShotgunAmmo}";
        MediumAmmoText.text = $" : {GlobalVariables.MediumAmmo}";
    }
}
