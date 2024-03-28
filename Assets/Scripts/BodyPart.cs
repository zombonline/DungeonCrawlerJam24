using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BodyPart : MonoBehaviour
{
    [SerializeField] string bodyPartName;
    bool interactable = false;

    [SerializeField] public int damageMin, damageMax;
    [SerializeField] public DamageType damageType;
 
    public void Hover()
    {
        FindObjectOfType<CombatUI>().SetEnemyInfo(GetComponentInParent<Enemy>(), this);
    }
    public void Click()
    {
        if (interactable)
        {
            FindObjectOfType<CombatManager>().BodyPartClicked(this);
        }
    }
    public void SetInteractable(bool value)
    {
        interactable = value;
    }
    public string GetBodyPartName()
    {
        return bodyPartName;
    }
}
