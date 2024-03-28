using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] string enemyName;
    [SerializeField] BodyPart[] bodyParts;
    Health health;

    void Awake()
    {
        health = GetComponent<Health>();
        AssignHitPointsInitialValue();
    }
    void AssignHitPointsInitialValue()
    {
        int newHitPoints = 0;   
        foreach (BodyPart bodyPart in bodyParts)
        {
            newHitPoints += bodyPart.GetComponent<Health>().GetInitialHitPoints();
        }
        newHitPoints = Mathf.RoundToInt(newHitPoints / 1.5f);
        health.SetInitialHitPoints(newHitPoints);
    }
    public string GetEnemyName()
    {
        return enemyName;
    }
    public void AttackPlayer()
    {
        var randomBodyPart = bodyParts[Random.Range(0, bodyParts.Length)];
        int damage = Random.Range(randomBodyPart.damageMin, randomBodyPart.damageMax);
        DamageType damageType = randomBodyPart.damageType;
        //damage var is returned from attack method so we can see if it was lessened or strengthened by resistances or weaknesses
        var newDamage = GameObject.FindWithTag("Player").GetComponent<Health>().TakeDamage(damage,damageType);
        var resultString = "";
        if(damage < newDamage)
        {
            resultString = "That looks like it hurt!";
        }
        else if(damage > newDamage)
        {
            resultString = "Your armor absorbed most of the damage!";
        }

        FindObjectOfType<CombatUI>().
        SetDialogueText("The " + enemyName + "'s " + randomBodyPart.GetBodyPartName() + " dealt " + newDamage + " " + damageType + " damage to you! " + resultString);
    }
    public void EnemyDeath()
    {
        FindObjectOfType<CombatManager>().DisableEnemy(this);
        Destroy(gameObject);
    }
}
