using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterHealthUI : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private PlayerMaxHealthStatResolver playerMaxHealthStatResolver;
    [SerializeField] private PlayerHealth playerHealth;

    [Header("UI Components")]
    [SerializeField] private Image healthBarImage;
    [Space]
    [SerializeField] private TextMeshProUGUI currentHealthText;
    [SerializeField] private TextMeshProUGUI maxHealthText;

    [Header("Settings")]
    [SerializeField, Range(0.01f, 10f)] private float lerpSpeed;

    private float LERP_THRESHOLD = 0.01f;

    private int maxHealth = 1;
    private int currentHealth = 0;

    private float targetFill;
    private float currentFill;

    private void OnEnable()
    {
        playerMaxHealthStatResolver.OnEntityStatInitialized += PlayerMaxHealthStatResolver_OnEntityStatInitialized;
        playerMaxHealthStatResolver.OnEntityStatUpdated += PlayerMaxHealthStatResolver_OnEntityStatUpdated;

        playerHealth.OnPlayerInitialized += PlayerHealth_OnPlayerInitialized;
        playerHealth.OnPlayerHealthTakeDamage += PlayerHealth_OnPlayerHealthTakeDamage;
        playerHealth.OnPlayerHeal += PlayerHealth_OnPlayerHeal;
        playerHealth.OnPlayerCurrentHealthClamped += PlayerHealth_OnPlayerCurrentHealthClamped;
    }

    private void OnDisable()
    {
        playerMaxHealthStatResolver.OnEntityStatInitialized -= PlayerMaxHealthStatResolver_OnEntityStatInitialized;
        playerMaxHealthStatResolver.OnEntityStatUpdated -= PlayerMaxHealthStatResolver_OnEntityStatUpdated;

        playerHealth.OnPlayerInitialized -= PlayerHealth_OnPlayerInitialized;
        playerHealth.OnPlayerHealthTakeDamage -= PlayerHealth_OnPlayerHealthTakeDamage;
        playerHealth.OnPlayerHeal -= PlayerHealth_OnPlayerHeal;
        playerHealth.OnPlayerCurrentHealthClamped -= PlayerHealth_OnPlayerCurrentHealthClamped;
    }

    private void Update()
    {
        HandleHealthBarLerping();
    }

    private void HandleHealthBarLerping()
    {
        if (Mathf.Abs(currentFill - targetFill) <= LERP_THRESHOLD) return;

        float newCurrentFill = (Mathf.Lerp(currentFill, targetFill, lerpSpeed * Time.deltaTime));

        SetCurrentFill(newCurrentFill);
        SetHealthBarFill(currentFill);
    }

    private void UpdateHealthValues(int currentHealth, int maxHealth)
    {
        SetCurrentHealth(currentHealth);
        SetMaxHealth(maxHealth);
    }

    private void UpdateUIByHealthValues()
    {
        SetHealthBarTexts(currentHealth, maxHealth); //Change Texts
        SetTargetFill(CalculateTargetFill(currentHealth, maxHealth));// Change Target Fill      
    }

    private void UpdateUIByHealthValuesImmediately()
    {
        SetHealthBarTexts(currentHealth, maxHealth); 
        SetTargetFill(CalculateTargetFill(currentHealth, maxHealth));   
        
        SetCurrentFill(targetFill);
        SetHealthBarFill(currentFill);
    }

    private void SetMaxHealth(int maxHealth) => this.maxHealth = maxHealth;
    private void SetCurrentHealth(int currentHealth) => this.currentHealth = currentHealth;
    private void SetTargetFill(float fill) => targetFill = fill;
    private void SetCurrentFill(float fill) => currentFill = fill;

    private float CalculateTargetFill(int currentHealth, int maxHealth) => (float)currentHealth / maxHealth;

    private void SetHealthBarFill(float fillAmount)
    {
        UIUtilities.SetImageFillRatio(healthBarImage, fillAmount);
    }

    private void SetHealthBarTexts(int currentHealth, int maxHealth)
    {
        currentHealthText.text = currentHealth.ToString();
        maxHealthText.text = maxHealth.ToString();
    }

    #region Subscriptions
    private void PlayerMaxHealthStatResolver_OnEntityStatInitialized(object sender, EntityIntStatResolver.OnStatEventArgs e)
    {
        UpdateHealthValues(currentHealth, e.value);
        UpdateUIByHealthValues();

        UpdateUIByHealthValuesImmediately();
    }

    private void PlayerMaxHealthStatResolver_OnEntityStatUpdated(object sender, EntityIntStatResolver.OnStatEventArgs e)
    {
        UpdateHealthValues(currentHealth, e.value);
        UpdateUIByHealthValues();
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    private void PlayerHealth_OnPlayerInitialized(object sender, EntityHealth.OnEntityInitializedEventArgs e)
    {
        UpdateHealthValues(e.currentHealth, maxHealth);
        UpdateUIByHealthValues();

        UpdateUIByHealthValuesImmediately();
    }
    private void PlayerHealth_OnPlayerHealthTakeDamage(object sender, EntityHealth.OnEntityHealthTakeDamageEventArgs e)
    {
        UpdateHealthValues(e.newHealth, e.maxHealth);
        UpdateUIByHealthValues();
    }

    private void PlayerHealth_OnPlayerHeal(object sender, EntityHealth.OnEntityHealEventArgs e)
    {
        UpdateHealthValues(e.newHealth, e.maxHealth);
        UpdateUIByHealthValues();
    }

    private void PlayerHealth_OnPlayerCurrentHealthClamped(object sender, EntityHealth.OnEntityCurrentHealthClampedEventArgs e)
    {
        UpdateHealthValues(e.currentHealth, e.maxHealth);
        UpdateUIByHealthValues();
    }
    #endregion
}
