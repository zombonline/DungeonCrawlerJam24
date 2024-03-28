using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Consumable", menuName = "Consumable")]
public class SOConsumable : ScriptableObject
{
    public string name, desc;
    public Sprite sprite;
    public int[] effectRange;
    public DamageType[] resistances, weaknesses;

    public void UseItem()
    {
        Debug.Log("Used " + name);
    }
}
