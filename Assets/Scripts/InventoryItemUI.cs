using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItemUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI itemName, itemDesc, itemValue;
    [SerializeField] Image itemImage;
    ScriptableObject item;
    public void SetItemInfo(ScriptableObject item)
    {
        this.item = item;
        // Check what kind of scriptable object it is
        switch(item)
        {
            case SOArmor armor:
                SetArmorInfo(armor);
                break;
            case SOWeapon weapon:
                SetWeaponInfo(weapon);
                break;
            case SOConsumable consumable:
                SetConsumableInfo(consumable);
                break;
        }
    }
    public void UseItem()
    {
        switch(item)
        {
            case SOArmor armor:
                if(!armor.UseItem())
                {
                    FindObjectOfType<CombatUI>().SetDialogueText("You already have " + armor.name + " armor equipped.");
                    return;
                }
                FindObjectOfType<CombatUI>().SetDialogueText("You equipped " + armor.name + " armor.");
                break;
            case SOWeapon weapon:
                if(!weapon.UseItem())
                {
                    FindObjectOfType<CombatUI>().SetDialogueText("You already have " + weapon.name + " weapon equipped.");
                    return;
                }
                FindObjectOfType<CombatUI>().SetDialogueText("You equipped " + weapon.name + " weapon.");
                break;
            case SOConsumable consumable:
                consumable.UseItem();
                FindObjectOfType<CombatUI>().SetDialogueText("You consumed " + consumable.name + ".");
                break;
        }
        FindObjectOfType<CombatUI>().SetPlayerInfo();
        FindObjectOfType<CombatManager>().ActionPerformed();
    }
    void SetArmorInfo(SOArmor armor)
    {
        itemName.text = armor.name;
        itemDesc.text = armor.desc;
        itemImage.sprite = armor.sprite;
        var resistancesString = "";
        foreach(DamageType resistance in armor.resistances)
        {
            resistancesString += resistance + ", ";
        }
        resistancesString = resistancesString.TrimEnd(',', ' ');
        var weaknessesString = "";
        foreach(DamageType weakness in armor.weaknesses)
        {
            weaknessesString += weakness + ", ";
        }
        weaknessesString = weaknessesString.TrimEnd(',', ' ');
        itemValue.text = "<b>DEF</b>:" + armor.effectRange[0].ToString() + "-" + armor.effectRange[1].ToString() +
         "\n<b>RES</b>: " + resistancesString + "\n<b>WK</b>: " + weaknessesString;
    }
    void SetWeaponInfo(SOWeapon weapon)
    {
        itemName.text = weapon.name;
        itemDesc.text = weapon.desc;
        itemImage.sprite = weapon.sprite;
        itemValue.text = "<b>DMG</b>: " + weapon.effectRange[0].ToString() + "-" + weapon.effectRange[1].ToString()
         + "\n<b>TYPE</b>:" + weapon.damageType;
    }
    void SetConsumableInfo(SOConsumable consumable)
    {
        itemName.text = consumable.name;
        itemDesc.text = consumable.desc;
        itemImage.sprite = consumable.sprite;
        itemValue.text = "<b>HP</b>:" + consumable.effectRange[0] + " - " + consumable.effectRange[1];
    }
}
