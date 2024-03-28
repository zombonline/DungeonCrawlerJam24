using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CombatState
{
    START,
    PLAYERTURN,
    ENEMYTURN,
    WON,
    LOST
}
public class CombatManager : MonoBehaviour
{
    [SerializeField] List<Enemy> activeEnemies = new List<Enemy>(), spawnableEnemies= new List<Enemy>();
    [SerializeField] RectTransform enemyHolderRect;
    public CombatState state;
    CombatUI combatUI;
    bool awaitingAction = false;
    bool actionPerformed = false;
    bool awaitingContinue = false;
    bool continuePressed = false;
    public static bool combatInProgress;

    public static void SetCombatInProgress(bool val)
    {
        combatInProgress = val;
    }
    void Awake()
    {
        combatUI = GetComponent<CombatUI>();
    }
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            BeginEncounter(CombatState.PLAYERTURN);
        }
    }
    public void BeginEncounter(CombatState initialState)
    {
        StartCoroutine(SetUpEncounter(initialState));
    }
    IEnumerator SetUpEncounter(CombatState initialState)
    {   
        state = CombatState.START;
        SetCombatInProgress(true);
        combatUI.FadeBlackScreen(.5f, 1f);
        yield return new WaitForSeconds(.5f);
        combatUI.ToggleCombatScreen(true);
        combatUI.FadeBlackScreen(.5f, 0f);
        yield return new WaitForSeconds(.5f);
        combatUI.SetPlayerInfo();
        state = initialState;
        SpawnEnemies();

        while(state != CombatState.WON && state != CombatState.LOST)
        {
            
            if(GameObject.FindWithTag("Player").GetComponent<Health>().GetHitPoints() <= 0)
            {
                state = CombatState.LOST;
                combatUI.SetDialogueText("You lost the battle!");
                yield return StartCoroutine(AwaitContinue());
                combatUI.ToggleCombatScreen(false);
            }
            if(activeEnemies.Count == 0)
            {
                state = CombatState.WON;
                combatUI.SetDialogueText("You won the battle!");
                yield return StartCoroutine(AwaitContinue());
                AwardLoot();
                yield return StartCoroutine(AwaitContinue());
                combatUI.ToggleCombatScreen(false);
            }
            if (state == CombatState.PLAYERTURN)
            {
                combatUI.SetDialogueText("Your turn!");
                yield return StartCoroutine(AwaitContinue());
                combatUI.SetDialogueText("Choose an action:");
                yield return StartCoroutine(AwaitAction());
                yield return StartCoroutine(AwaitContinue());
                state = CombatState.ENEMYTURN;
            }
            else if(state == CombatState.ENEMYTURN)
            {
                combatUI.SetDialogueText("Enemy's turn!");
                yield return StartCoroutine(AwaitContinue());
                foreach(Enemy enemy in activeEnemies)
                {
                    enemy.AttackPlayer();
                    combatUI.SetPlayerInfo();
                    yield return StartCoroutine(AwaitContinue());
                } 
                state = CombatState.PLAYERTURN;
            }
        }
        foreach(Enemy enemy in activeEnemies)
        {
            DisableEnemy(enemy);
        }
    }

    void SpawnEnemies()
    {
        var randomEnemyCount = Random.Range(1, 4);
        for (int i = 0; i < randomEnemyCount; i++)
        {
            var randomEnemy = spawnableEnemies[Random.Range(0, spawnableEnemies.Count)];
            var newEnemy = Instantiate(randomEnemy, enemyHolderRect);
            newEnemy.GetComponent<RectTransform>().anchoredPosition = new Vector3(enemyHolderRect.rect.width/(randomEnemyCount+1) * (i+1), 0, 0);
            activeEnemies.Add(newEnemy);
        }

    }
    public void DisableEnemy(Enemy enemy)
    {
        activeEnemies.Remove(enemy);
        Destroy(enemy.gameObject);
    }
    private void AwardLoot()
    {
        ScriptableObject award = FindObjectOfType<ItemSpawner>().GetRandomItem();
        string itemName = "";
        //check what kind of scriptable object it is
        switch (award)
        {
            case SOWeapon weapon:
                itemName = weapon.name;
                break;
            case SOArmor armor:
                itemName = armor.name;
                break;
            case SOConsumable consumable:
                itemName = consumable.name;
                break;
        }
        combatUI.SetDialogueText("You found a " + itemName + "!");
        FindObjectOfType<PlayerCombat>().AddToInventory(award);
    }

    public void PlayerAttack(BodyPart bodyPart)
    {
        SOWeapon equippedWeapon = FindObjectOfType<PlayerCombat>().GetEquippedWeapon();

        int damage = Random.Range(equippedWeapon.effectRange[0], equippedWeapon.effectRange[1]);
        DamageType damageType = equippedWeapon.damageType;

        var newDamage =bodyPart.GetComponent<Health>().TakeDamage(damage,damageType);
        var resultString = "";
        if(damage < newDamage)
        {
            resultString = "The enemy screeches in pain!";
        }
        else if(damage > newDamage)
        {
            resultString = "Your weapon didn't do much damage!";
        }
        combatUI.
        SetDialogueText("You hit " + bodyPart.transform.parent.GetComponent<Enemy>().GetEnemyName()
         + "'s " + bodyPart.GetBodyPartName() + " for " + newDamage + " " + damageType + " damage! " + resultString);
    }
    private IEnumerator AwaitContinue()
    {
        awaitingContinue = true;
        yield return new WaitUntil(() => continuePressed);
        continuePressed = false;
        awaitingContinue = false;
    }
    private IEnumerator AwaitAction()
    {
        combatUI.ToggleActionButtons(true);
        awaitingAction = true;
        yield return new WaitUntil(() => actionPerformed);
        actionPerformed = false;
        awaitingAction = false;
    }

    public void ContinuePressed()
    {
        if(!awaitingContinue)
        {
            return;
        }
        continuePressed = true;
    }
    public void ActionPerformed()
    {
        if (!awaitingAction)
        {
            return;
        }
        actionPerformed = true;
        SetBodyPartTargetting(false);
        combatUI.ToggleActionButtons(false);
        combatUI.ToggleInventoryScreen(false);
    }

    private void SetBodyPartTargetting(bool val)
    {
        foreach (Enemy enemy in FindObjectsOfType<Enemy>())
        {
            foreach (BodyPart bodyPart in enemy.GetComponentsInChildren<BodyPart>())
            {
                bodyPart.SetInteractable(val);
            }
        }
    }
    public void AttackPressed()
    {
        if(!awaitingAction)
        {
            return;
        }
        combatUI.ToggleInventoryScreen(false);
        combatUI.SetDialogueText("Where are you hitting?");

        SetBodyPartTargetting(true);
    }
    public void InventoryPressed()
    {
        if(!awaitingAction)
        {
            return;
        }
        combatUI.SetDialogueText("What are you using?");
        SetBodyPartTargetting(false);
        combatUI.ToggleInventoryScreen(true);
        SetBodyPartTargetting(false);
    }
    public void BodyPartClicked(BodyPart bodyPart)
    {
        PlayerAttack(bodyPart);
        combatUI.SetEnemyInfo(bodyPart.transform.parent.GetComponent<Enemy>(), bodyPart);
        ActionPerformed();
    }
}
