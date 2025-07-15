using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EntityHealthBarUI : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private EntityMaxHealthStatResolver entityMaxHealthStatResolver;
    [SerializeField] private EntityHealth entityHealth;

    [Header("UI Components")]
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private Image healthBarImage;
    [Space]
    [SerializeField] private TextMeshProUGUI currentHealthText;
    [SerializeField] private TextMeshProUGUI maxHealthText;

    [Header("Settings")]
    [SerializeField, Range(0.01f, 10f)] private float lerpSpeed;
    [SerializeField] private bool startDisabled;

    private float LERP_THRESHOLD = 0.01f;

    private int maxHealth = 1;
    private int currentHealth = 0;

    private float targetFill;
    private float currentFill;

    private void OnEnable()
    {
        entityMaxHealthStatResolver.OnEntityStatInitialized += EntityMaxHealthStatResolver_OnEntityStatInitialized;
        entityMaxHealthStatResolver.OnEntityStatUpdated += EntityMaxHealthStatResolver_OnEntityStatUpdated;

        entityHealth.OnEntityInitialized += EntityHealth_OnEntityInitialized;
        entityHealth.OnEntityHealthTakeDamage += EntityHealth_OnEntityHealthTakeDamage;
        entityHealth.OnEntityHeal += EntityHealth_OnEntityHeal;
        entityHealth.OnEntityCurrentHealthClamped += EntityHealth_OnEntityCurrentHealthClamped;

        entityHealth.OnEntityDeath += EntityHealth_OnEntityDeath;
    }

    private void OnDisable()
    {
        entityMaxHealthStatResolver.OnEntityStatInitialized -= EntityMaxHealthStatResolver_OnEntityStatInitialized;
        entityMaxHealthStatResolver.OnEntityStatUpdated -= EntityMaxHealthStatResolver_OnEntityStatUpdated;

        entityHealth.OnEntityInitialized -= EntityHealth_OnEntityInitialized;
        entityHealth.OnEntityHealthTakeDamage -= EntityHealth_OnEntityHealthTakeDamage;
        entityHealth.OnEntityHeal -= EntityHealth_OnEntityHeal;
        entityHealth.OnEntityCurrentHealthClamped -= EntityHealth_OnEntityCurrentHealthClamped;

        entityHealth.OnEntityDeath -= EntityHealth_OnEntityDeath;
    }

    private void Start()
    {
        if (startDisabled) DisableHealthBar();
    }

    private void Update()
    {
        HandleHealthBarLerping();
    }

    private void EnableHealthBar() => UIUtilities.SetCanvasGroupAlpha(canvasGroup, 1f);
    private void DisableHealthBar() => UIUtilities.SetCanvasGroupAlpha(canvasGroup, 0f);

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
    private void EntityMaxHealthStatResolver_OnEntityStatInitialized(object sender, EntityIntStatResolver.OnStatEventArgs e)
    {
        UpdateHealthValues(currentHealth, e.value);
        UpdateUIByHealthValues();

        UpdateUIByHealthValuesImmediately();
    }

    private void EntityMaxHealthStatResolver_OnEntityStatUpdated(object sender, EntityIntStatResolver.OnStatEventArgs e)
    {
        UpdateHealthValues(currentHealth, e.value);
        UpdateUIByHealthValues();
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    private void EntityHealth_OnEntityInitialized(object sender, EntityHealth.OnEntityInitializedEventArgs e)
    {
        UpdateHealthValues(e.currentHealth, maxHealth);
        UpdateUIByHealthValues();

        UpdateUIByHealthValuesImmediately();
    }
    private void EntityHealth_OnEntityHealthTakeDamage(object sender, EntityHealth.OnEntityHealthTakeDamageEventArgs e)
    {
        UpdateHealthValues(e.newHealth, e.maxHealth);
        UpdateUIByHealthValues();

        EnableHealthBar();
    }

    private void EntityHealth_OnEntityHeal(object sender, EntityHealth.OnEntityHealEventArgs e)
    {
        UpdateHealthValues(e.newHealth, e.maxHealth);
        UpdateUIByHealthValues();

        EnableHealthBar();
    }

    private void EntityHealth_OnEntityCurrentHealthClamped(object sender, EntityHealth.OnEntityCurrentHealthClampedEventArgs e)
    {
        UpdateHealthValues(e.currentHealth, e.maxHealth);
        UpdateUIByHealthValues();

        EnableHealthBar();
    }

    private void EntityHealth_OnEntityDeath(object sender, EntityHealth.OnEntityDeathEventArgs e)
    {
        DisableHealthBar();
    }
    #endregion
}
