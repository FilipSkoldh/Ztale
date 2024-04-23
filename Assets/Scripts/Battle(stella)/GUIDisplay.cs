using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GUIDisplay : MonoBehaviour
{
    private TextMeshProUGUI hpText;
    private TextMeshProUGUI weaponText;
    private TextMeshProUGUI equipmentText;

    private void Awake()
    {
        hpText = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        weaponText = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        equipmentText = transform.GetChild(2).GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        hpText.text = $" Hp: {GlobalVariables.Hp} / {GlobalVariables.MaxHp}";
        if (GlobalVariables.EquippedWeapon == null)
            weaponText.text = " Weapon:";
        else 
            weaponText.text = $" Weapon: {GlobalVariables.EquippedWeapon.name}";
        if (GlobalVariables.EquippedEquipment == null)
            equipmentText.text = " Equipment:";
        else
            equipmentText.text = $" Equipment: {GlobalVariables.EquippedEquipment.name}";
    }
}
