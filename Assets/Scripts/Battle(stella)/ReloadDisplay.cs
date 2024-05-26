using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ReloadDisplay : MonoBehaviour
{
    private TextMeshProUGUI text;

    private void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
    }
    void Update()
    {
        if (GlobalVariables.EquippedWeaponAmmo != 0 || GlobalVariables.EquippedEquipment == null)
            text.text = "SHOOT";
        else
            text.text = "RELOAD";
    }
}
