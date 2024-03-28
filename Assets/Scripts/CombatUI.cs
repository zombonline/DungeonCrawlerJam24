using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CombatUI : MonoBehaviour
{
    [SerializeField] RectTransform combatScreen;
    [SerializeField] TextMeshProUGUI dialogueText;

    [Header("Enemy Box")]
    [SerializeField] TextMeshProUGUI enemyName;
    [SerializeField] TextMeshProUGUI bodyPartName, bodyPartEffects;
    [SerializeField] Slider enemyHealth, bodyPartHealth;
    [SerializeField] Image bodyPartImage;

    [SerializeField] RectTransform enemyBox;

    [SerializeField] Button[] actionButtons;    

    [Header("Player Info")]
    [SerializeField] RectTransform playerInfoBox;
    [SerializeField] RectTransform postCombatScreen, posOverworldScreem;
    [SerializeField] TextMeshProUGUI playerName;
    [SerializeField] Slider playerHealth;
    [SerializeField] Image playerImage, equippedWeaponImage, equippedArmorImage;

    [SerializeField] RectTransform itemBox;
    [SerializeField] ScrollRect itemBoxScrollRect;
    [SerializeField] RectTransform itemTemplate;

    [SerializeField] RectTransform blackScreen;

    

    public void SetDialogueText(string text)
    {
        dialogueText.text = text;
    }

    public void ToggleActionButtons(bool value)
    {
        foreach (Button button in actionButtons)
        {
            button.gameObject.SetActive(value);
        }
    }

    public void ContinueButton()
    {
        GetComponent<CombatManager>().ContinuePressed();
    }
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            ContinueButton();
        }
    }
    public void SetEnemyInfo(Enemy enemy, BodyPart bodyPart)
    {
        if(enemy == null)
        {
            enemyBox.gameObject.SetActive(false);
            return;
        }
        enemyBox.gameObject.SetActive(true);
        enemyName.text = enemy.GetEnemyName();
        bodyPartName.text = bodyPart.GetBodyPartName();
        bodyPartImage.sprite = bodyPart.GetComponent<Image>().sprite;
        enemyHealth.maxValue = enemy.GetComponent<Health>().GetInitialHitPoints();
        enemyHealth.value = enemy.GetComponent<Health>().GetHitPoints();
        bodyPartHealth.maxValue = bodyPart.GetComponent<Health>().GetInitialHitPoints();
        bodyPartHealth.value = bodyPart.GetComponent<Health>().GetHitPoints();
        
        var resistancesString = "";
        foreach(DamageType resistance in bodyPart.GetComponent<Health>().resistances)
        {
            resistancesString += resistance + ", ";
        }
        resistancesString = resistancesString.TrimEnd(',', ' ');
        var weaknessesString = "";
        foreach (DamageType weakness in bodyPart.GetComponent<Health>().weaknesses)
        {
            weaknessesString += weakness + ", ";
        }
        weaknessesString = weaknessesString.TrimEnd(',', ' ');
        
        bodyPartEffects.text = 
        "<B>RES</B>: " + resistancesString +
        "\n<B>WK</B>: " + weaknessesString +
        "\n\n<B>DMG</B>: " + bodyPart.damageMin + "-" + bodyPart.damageMax +
        "\n<B>TYPE</B>: " + bodyPart.damageType;
    }
    public void ToggleCombatScreen(bool val)
    {
        enemyBox.gameObject.SetActive(val);
        combatScreen.gameObject.SetActive(val);
        if(val)
        {
            playerInfoBox.transform.parent = postCombatScreen;
            playerInfoBox.anchoredPosition = Vector3.zero;
        }
        else
        {
            playerInfoBox.transform.parent = posOverworldScreem;
            playerInfoBox.anchoredPosition = Vector3.zero;
        }
    }
    public void SetPlayerInfo()
    {
        PlayerCombat player = FindObjectOfType<PlayerCombat>();
        playerHealth.maxValue = player.GetComponent<Health>().GetInitialHitPoints();
        playerHealth.value = player.GetComponent<Health>().GetHitPoints();
        equippedArmorImage.sprite = player.GetEquippedArmor().sprite;
        equippedWeaponImage.sprite = player.GetEquippedWeapon().sprite;
    }

    public void ToggleInventoryScreen(bool val)
    {
        itemBox.gameObject.SetActive(val);
        if(val)
        {
            PopulateItemBox();
        }
    }
    public void PopulateItemBox()
    {
        foreach (Transform child in itemBoxScrollRect.content)
        {
            Destroy(child.gameObject);
        }
        var inventory = FindObjectOfType<PlayerCombat>().GetInventory();
        itemBoxScrollRect.content.sizeDelta = new Vector2(itemBoxScrollRect.content.sizeDelta.x, inventory.Count * itemTemplate.rect.size.y);
        for(int i = 0; i < inventory.Count; i++)
        {
            RectTransform newItem = Instantiate(itemTemplate, itemBoxScrollRect.content);
            newItem.anchoredPosition = (Vector3.up * -newItem.rect.size.y * (i+1)) + Vector3.up * newItem.rect.size.y/2;
            newItem.GetComponent<InventoryItemUI>().SetItemInfo(inventory[i]);
            newItem.gameObject.SetActive(true);
        }
    }

    public void FadeBlackScreen(float fadeTime, float targetAlpha)
    {
        StartCoroutine(FadeBlackScreenRoutine(fadeTime, targetAlpha));
    }

    IEnumerator FadeBlackScreenRoutine(float fadeTime, float targetAlpha)
    {
        float dir = targetAlpha - blackScreen.GetComponent<Image>().color.a;
        while(Mathf.Abs(blackScreen.GetComponent<Image>().color.a - targetAlpha) > 0.01f)
        {
            Color newColor = blackScreen.GetComponent<Image>().color;
            newColor.a += dir * Time.deltaTime / fadeTime;
            blackScreen.GetComponent<Image>().color = newColor;
            yield return null;
        }
    }
}
