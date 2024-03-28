using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    ScriptableObject item;

    public void SetItem(ScriptableObject item)
    {
        this.item = item;
    }
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            FindObjectOfType<PlayerCombat>().AddToInventory(item);
            Destroy(gameObject);
        }
    }
}
