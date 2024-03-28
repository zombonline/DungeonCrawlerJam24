using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [SerializeField] SOWeapon equippedWeapon;
    [SerializeField] SOArmor equippedArmor;

    [SerializeField] List<ScriptableObject> inventory = new List<ScriptableObject>();


    public void EquipWeapon(SOWeapon weapon)
    {
        equippedWeapon = weapon;
    }
    public void EquipArmor(SOArmor armor)
    {
        equippedArmor = armor;
        GetComponent<Health>().SetEffects(armor.resistances, armor.weaknesses);
    }
    public SOArmor GetEquippedArmor()
    {
        return equippedArmor;
    }
    public SOWeapon GetEquippedWeapon()
    {
        return equippedWeapon;
    }
    public List<ScriptableObject> GetInventory()
    {
        return inventory;
    }
    public void AddToInventory(ScriptableObject item)
    {
        inventory.Add(item);
    }
    public void RemoveFromInventory(ScriptableObject item)
    {
        inventory.Remove(item);
    }
}
