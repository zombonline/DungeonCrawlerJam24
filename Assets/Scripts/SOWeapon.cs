using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum DamageType
{
    Physical,
    Fire,
    Ice,
    Dark,
    Light
}
[CreateAssetMenu(fileName = "New Weapon", menuName = "Weapon")]
public class SOWeapon : ScriptableObject
{
    public string name, desc;
    public Sprite sprite;
    public int[] effectRange;
    public DamageType damageType;

    public bool UseItem()
    {
        PlayerCombat player = FindObjectOfType<PlayerCombat>();
        if(player.GetEquippedWeapon() == this)
        {
            return false;
        }
        player.EquipWeapon(this);
        return true;
    }
}
