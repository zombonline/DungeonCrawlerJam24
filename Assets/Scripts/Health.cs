using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    [SerializeField] int initialHitPoints;
    int hitPoints;
    [SerializeField] UnityEvent onDeath, onDamageTaken;
    [SerializeField] public DamageType[] resistances, weaknesses;
    void Start()
    {
        if(initialHitPoints <= 0)
        {
            return;
        }
        hitPoints = initialHitPoints;
    }
    public void SetInitialHitPoints(int value)
    {
        initialHitPoints = value;
        hitPoints = initialHitPoints;
    }
    public int GetHitPoints()
    {
        return hitPoints;
    }

    public int GetInitialHitPoints()
    {
        return initialHitPoints;
    }

    public int TakeDamage(int damage, DamageType damageType)
    {
        Debug.Log(damage);
        Debug.Log(resistances.Contains(damageType));
        if(GetComponent<PlayerCombat>() != null)
        {
            if (GetComponent<PlayerCombat>() != null)
            {
                var armor = GetComponent<PlayerCombat>().GetEquippedArmor();
                var defensePercentage = Random.Range(armor.effectRange[0], armor.effectRange[1]);
                damage = Mathf.RoundToInt(damage * (1 - (defensePercentage / 100)));
            }
        }
        if(resistances.Contains(damageType))
        {
            damage = Mathf.RoundToInt(damage / 2);
        }
        else if(weaknesses.Contains(damageType))
        {
            damage = damage * 2;
        }
        Debug.Log(damage);
        hitPoints -= damage;
        onDamageTaken.Invoke();
        if(GetComponent<BodyPart>() != null)
        {
            transform.parent.GetComponent<Health>().TakeDamage(damage, damageType);
        }
        if (hitPoints <= 0)
        {
            Die();
        }
        return damage;
    }

    void Die()
    {
        onDeath.Invoke();
    }

    public void SetEffects(DamageType[] resistances, DamageType[] weaknesses)
    {
        this.resistances = resistances;
        this.weaknesses = weaknesses;
    }

}
