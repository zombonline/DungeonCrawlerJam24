using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Armor", menuName = "Armor")]
public class SOArmor : ScriptableObject
{
    public string name, desc;
    public Sprite sprite;
    public int[] effectRange;
    public DamageType[] resistances, weaknesses;
    public bool UseItem()
    {
        PlayerCombat player = FindObjectOfType<PlayerCombat>();
        if(player.GetEquippedArmor() == this)
        {
            return false;
        }
        player.EquipArmor(this);
        return true;
    }
}
