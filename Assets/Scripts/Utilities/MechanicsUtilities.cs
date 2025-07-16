using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MechanicsUtilities
{
    public const float PERSPECTIVE_SCALE_X = 1f;
    public const float PERSPECTIVE_SCALE_Y = 1f;

    private const int ARMOR_THRESHOLD_50_PERCENT = 10;

    private const float COOLDOWN_REDUCTION_50_PERCENT = 1f;
    private const float ABILITY_COOLDOWN_MIN_VALUE = 1f;

    private const int EXECUTE_DAMAGE = 999;

    #region Clamping Consts
    public const int MIN_MAX_HEALTH = 1;
    public const int MAX_MAX_HEALTH = 1000;

    public const int MIN_ARMOR = 0;
    public const int MAX_ARMOR = 1000;

    public const float MIN_DODGE_CHANCE = 0f;
    public const float MAX_DODGE_CHANCE = 0.9f;

    public const float MIN_MOVEMENT_SPEED = 1f;
    public const float MAX_MOVEMENT_SPEED = 20f;

    public const float MIN_ATTACK_DAMAGE = 1f;
    public const float MAX_ATTACK_DAMAGE = 20f;

    public const float MIN_ATTACK_SPEED = 0.3f;
    public const float MAX_ATTACK_SPEED = 10f;

    public const float MIN_CRIT_CHANCE = 0f;
    public const float MAX_CRIT_CHANCE = 1f;

    public const float MIN_CRIT_MULT = 0.5f;
    public const float MAX_CRIT_MULT = 3f;

    public const int MIN_HEALTH_REGEN = 0;
    public const int MAX_HEALTH_REGEN = 20;

    public const float MIN_LIFESTEAL = 0f;
    public const float MAX_LIFESTEAL = 1f;

    public const float MIN_COOLDOWN_REDUCTION = 0f;
    public const float MAX_COOLDOWN_REDUCTION = 1f;

    public const int MIN_GOLD_EARNED_STAT = 0;
    public const int MAX_GOLD_EARNED_STAT = 1000;
    #endregion

    #region Perspective
    public static Vector2 ScaleVector2ToPerspective(Vector2 baseVector)
    {
        Vector2 scaledVector = new Vector2(baseVector.x * PERSPECTIVE_SCALE_X, baseVector.y * PERSPECTIVE_SCALE_Y);
        return scaledVector;
    }
    #endregion

    #region Const GetMethods
    public static int GetArmor50PercentThreshold() => ARMOR_THRESHOLD_50_PERCENT;
    public static int GetExecuteDamage() => EXECUTE_DAMAGE;
    public static float GetAbilityCooldownMinValue() => ABILITY_COOLDOWN_MIN_VALUE;
    #endregion

    #region Damage Evaluation & Processing

    public static bool EvaluateCritAttack(float critChance, bool overrideCrit = false, bool overrideNotCrit = false)
    {
        if (overrideCrit) return true;
        if (overrideNotCrit) return false;

        float randomNumber = Random.Range(0f, 1f);

        if (critChance >= randomNumber) return true;
        return false;
    }

    public static bool EvaluateDodgeChance(float dodgeChance)
    {
        float randomNumber = Random.Range(0f, 1f);

        if (dodgeChance >= randomNumber) return true;
        return false;
    }

    public static int MitigateDamageByArmor(int baseDamage, int armor)
    {
        if(armor < 0) armor = 0;

        float rawResultingDamage = (float) baseDamage / (1 + armor/ARMOR_THRESHOLD_50_PERCENT); // ARMOR MITIGATION FORMULA!
        int roundedResultingDamage = Mathf.CeilToInt(rawResultingDamage);

        return roundedResultingDamage;
    }

    public static int CalculateCritDamage(int nonCritDamage, float attackCritDamageMultiplier)
    {
        float critDamage = nonCritDamage * attackCritDamageMultiplier;
        int roundedCritDamage = Mathf.CeilToInt(critDamage);

        return roundedCritDamage;
    }
    #endregion

    #region Damage Dealing

    public static void DealDamageInAreas(List<Vector2> positions, float areaRadius, DamageData damageData , LayerMask layermask)
    {
        List<Transform> detectedTransforms = GeneralUtilities.DetectTransformsInMultipleRanges(positions, areaRadius, layermask);
        List<IHasHealth> entityHealthsInRange = GeneralUtilities.TryGetGenericsFromTransforms<IHasHealth>(detectedTransforms);

        foreach (IHasHealth iHasHealth in entityHealthsInRange)
        {
            iHasHealth.TakeDamage(damageData);
        }
    }

    public static void DealDamageInAreas(List<Vector2> positions, float areaRadius, DamageData damageData, LayerMask layermask, List<Transform> exeptionTransforms)
    {
        List<Transform> detectedTransforms = GeneralUtilities.DetectTransformsInMultipleRanges(positions, areaRadius, layermask);

        foreach(Transform exceptionTransform in exeptionTransforms)
        {
            detectedTransforms.Remove(exceptionTransform);
        }

        List<IHasHealth> entityHealthsInRange = GeneralUtilities.TryGetGenericsFromTransforms<IHasHealth>(detectedTransforms);

        foreach (IHasHealth iHasHealth in entityHealthsInRange)
        {
            iHasHealth.TakeDamage(damageData);
        }
    }

    public static void DealDamageInArea(Vector2 position, float areaRadius, DamageData damageData, LayerMask layermask)
    {
        List<Transform> detectedTransforms = GeneralUtilities.DetectTransformsInRange(position, areaRadius, layermask);
        List<IHasHealth> entityHealthsInRange = GeneralUtilities.TryGetGenericsFromTransforms<IHasHealth>(detectedTransforms);

        foreach (IHasHealth iHasHealth in entityHealthsInRange)
        {
            iHasHealth.TakeDamage(damageData);
        }
    }

    public static void DealDamageInArea(Vector2 position, float areaRadius, DamageData damageData, LayerMask layermask, List<Transform> exceptionTransforms)
    {
        List<Transform> detectedTransforms = GeneralUtilities.DetectTransformsInRange(position, areaRadius, layermask);

        foreach (Transform exceptionTransform in exceptionTransforms)
        {
            detectedTransforms.Remove(exceptionTransform);
        }

        List<IHasHealth> entityHealthsInRange = GeneralUtilities.TryGetGenericsFromTransforms<IHasHealth>(detectedTransforms);

        foreach (IHasHealth iHasHealth in entityHealthsInRange)
        {
            iHasHealth.TakeDamage(damageData);
        }
    }

    public static bool DealDamageToTransform(Transform transform, DamageData damageData)
    {
        bool damaged = false;

        if(GeneralUtilities.TryGetGenericFromTransform<IHasHealth>(transform, out var iHasHealth))
        {
            damaged = iHasHealth.TakeDamage(damageData);
        }

        return damaged;
    }

    #endregion

    #region Projectiles
    public static Vector2 DeviateShootDirection(Vector2 shootDirection, float dispersionAngle)
    {
        float randomAngle = Random.Range(-dispersionAngle, dispersionAngle);

        Vector2 deviatedDirection = GeneralUtilities.RotateVector2ByAngleDegrees(shootDirection, randomAngle);
        deviatedDirection.Normalize();

        return deviatedDirection;
    }
    #endregion

    #region Stats
    public static float ClampNumericStat(float baseValue, NumericStatType numericStatType)
    {
        switch (numericStatType)
        {
            case NumericStatType.MaxHealth:
                return ClampNumericStat(baseValue, MIN_MAX_HEALTH, MAX_MAX_HEALTH);
            case NumericStatType.Armor:
                return ClampNumericStat(baseValue, MIN_ARMOR, MAX_ARMOR);
            case NumericStatType.DodgeChance:
                return ClampNumericStat(baseValue, MIN_DODGE_CHANCE, MAX_DODGE_CHANCE);
            case NumericStatType.MovementSpeed:
                return ClampNumericStat(baseValue, MIN_MOVEMENT_SPEED, MAX_MOVEMENT_SPEED);
            case NumericStatType.AttackDamage:
                return ClampNumericStat(baseValue, MIN_ATTACK_DAMAGE, MAX_ATTACK_DAMAGE);
            case NumericStatType.AttackSpeed:
                return ClampNumericStat(baseValue, MIN_ATTACK_SPEED, MAX_ATTACK_SPEED);
            case NumericStatType.AttackCritChance:
                return ClampNumericStat(baseValue, MIN_CRIT_CHANCE, MAX_CRIT_CHANCE);
            case NumericStatType.AttackCritDamageMultiplier:
                return ClampNumericStat(baseValue, MIN_CRIT_MULT, MAX_CRIT_MULT);
            case NumericStatType.HealthRegen:
                return ClampNumericStat(baseValue, MIN_HEALTH_REGEN, MAX_HEALTH_REGEN);
            case NumericStatType.Lifesteal:
                return ClampNumericStat(baseValue, MIN_LIFESTEAL, MAX_LIFESTEAL);
            case NumericStatType.CooldownReduction:
                return ClampNumericStat(baseValue, MIN_COOLDOWN_REDUCTION, MAX_COOLDOWN_REDUCTION);
            case NumericStatType.Gold:
                return ClampNumericStat(baseValue, MIN_GOLD_EARNED_STAT, MAX_GOLD_EARNED_STAT);
            default: //Default is Unclamped
                return baseValue;
        }
    }

    public static float ClampNumericStat(float baseValue, float minValue, float maxValue)
    {
        float clampedValue = baseValue > maxValue ? maxValue : baseValue;
        clampedValue = clampedValue < minValue ? minValue : clampedValue;

        return clampedValue;
    }

    public static NumericEmbeddedStat GenerateProportionalNumericStatPerStack(int stacks, NumericEmbeddedStat numericEmbeddedStatPerStack)
    {
        NumericEmbeddedStat stackedEmbeddedStat = new NumericEmbeddedStat
        {
            numericStatType = numericEmbeddedStatPerStack.numericStatType,
            numericStatModificationType = numericEmbeddedStatPerStack.numericStatModificationType,
            value = numericEmbeddedStatPerStack.value * stacks
        };

        return stackedEmbeddedStat;
    }

    #endregion

    #region Abilities
    public static float ProcessAbilityCooldown(float baseCooldown, float normalizedCooldownReduction)
    {
        if(normalizedCooldownReduction < 0f) normalizedCooldownReduction = 0f;

        float processedCooldown = baseCooldown * (1 - normalizedCooldownReduction);

        /*
        float processedCooldown = baseCooldown / (1 + normalizedCooldownReduction/COOLDOWN_REDUCTION_50_PERCENT); //COOLDOWN REDUCTION ALT FORMULA!
        */

        processedCooldown = processedCooldown < ABILITY_COOLDOWN_MIN_VALUE ? ABILITY_COOLDOWN_MIN_VALUE : processedCooldown;

        return processedCooldown;
    }

    public static AbilityLevel GetNextAbilityLevel(AbilityLevel previousAbilityLevel)
    {
        switch (previousAbilityLevel)
        {
            case AbilityLevel.NotLearned:
            default:
                return AbilityLevel.Level1;
            case AbilityLevel.Level1:
                return AbilityLevel.Level2;
            case AbilityLevel.Level2:
                return AbilityLevel.Level3;
            case AbilityLevel.Level3:
                return AbilityLevel.Level3;
        }
    }
    #endregion

    #region PhysicPush
    public static void PushAllEntitiesFromPoint(Vector2 originPoint, PhysicPushData pushData, float actionRadius, LayerMask pushLayerMask)
    {
        List<Transform> detectedTransforms = GeneralUtilities.DetectTransformsInRange(originPoint, actionRadius, pushLayerMask);
        List<EntityPhysicPush> entityPhysicPushes = new List<EntityPhysicPush>();

        foreach (Transform detectedTransform in detectedTransforms)
        {
            EntityPhysicPush entityPhysicPush = detectedTransform.GetComponentInChildren<EntityPhysicPush>();

            if (entityPhysicPush == null) continue;

            entityPhysicPushes.Add(entityPhysicPush);
        }

        foreach (EntityPhysicPush entityPhysicPush in entityPhysicPushes)
        {
            entityPhysicPush.PushEnemyFromPoint(originPoint, pushData);
        }
    }

    public static void PushAllEntitiesFromPoint(Vector2 originPoint, PhysicPushData pushData, LayerMask pushLayerMask, float actionRadius, List<Transform> exeptionTransforms)
    {
        List<Transform> detectedTransforms = GeneralUtilities.DetectTransformsInRange(originPoint, actionRadius, pushLayerMask);

        foreach (Transform exceptionTransform in exeptionTransforms)
        {
            detectedTransforms.Remove(exceptionTransform);
        }

        List<EntityPhysicPush> entityPhysicPushes = new List<EntityPhysicPush>();

        foreach (Transform detectedTransform in detectedTransforms)
        {
            EntityPhysicPush entityPhysicPush = detectedTransform.GetComponentInChildren<EntityPhysicPush>();

            if (entityPhysicPush == null) continue;

            entityPhysicPushes.Add(entityPhysicPush);
        }

        foreach (EntityPhysicPush entityPhysicPush in entityPhysicPushes)
        {
            entityPhysicPush.PushEnemyFromPoint(originPoint, pushData);
        }
    }

    public static void PushEntitiesInAreasFromPoint(Vector2 pushOrigin, PhysicPushData pushData, List<Vector2> positions, float actionRadiusPerPosition, LayerMask pushLayerMask)
    {
        List<Transform> detectedTransforms = GeneralUtilities.DetectTransformsInMultipleRanges(positions, actionRadiusPerPosition, pushLayerMask);
        List<EntityPhysicPush> entityPhysicPushes = new List<EntityPhysicPush>();

        foreach (Transform detectedTransform in detectedTransforms)
        {
            EntityPhysicPush entityPhysicPush = detectedTransform.GetComponentInChildren<EntityPhysicPush>();

            if (entityPhysicPush == null) continue;

            entityPhysicPushes.Add(entityPhysicPush);
        }

        foreach (EntityPhysicPush entityPhysicPush in entityPhysicPushes)
        {
            entityPhysicPush.PushEnemyFromPoint(pushOrigin, pushData);
        }
    }
    #endregion

    #region StatusEffects

    #region Slow

    public static void TemporalSlowInArea(Vector2 position, float areaRadius, TemporalSlowStatusEffect temporalSlowStatusEffect, LayerMask layermask)
    {
        List<Transform> detectedTransforms = GeneralUtilities.DetectTransformsInRange(position, areaRadius, layermask);

        foreach (Transform detectedTransform in detectedTransforms)
        {
            TemporalSlowEntity(detectedTransform, temporalSlowStatusEffect);
        }
    }

    public static void TemporalSlowInAreas(List<Vector2> positions, float areaRadius, TemporalSlowStatusEffect temporalSlowStatusEffect, LayerMask layermask)
    {
        List<Transform> detectedTransforms = GeneralUtilities.DetectTransformsInMultipleRanges(positions, areaRadius, layermask);

        foreach (Transform detectedTransform in detectedTransforms)
        {
            TemporalSlowEntity(detectedTransform, temporalSlowStatusEffect);
        }
    }

    public static void TemporalSlowEntity(Transform entityTransform, TemporalSlowStatusEffect temporalSlowStatusEffect)
    {
        EntitySlowStatusEffectHandler slowHandler = entityTransform.GetComponentInChildren<EntitySlowStatusEffectHandler>();

        if(slowHandler == null) return;

        slowHandler.TemporalAddSlowStatusEffect(temporalSlowStatusEffect);
    }
    #endregion

    #endregion

    #region Odds
    public static bool GetProbability(float normalizedProbability)
    {
        if(normalizedProbability <= 0) return false;
        if(normalizedProbability >= 1) return true;

        float randomNumber = Random.Range(0f, 1f);

        if (normalizedProbability >= randomNumber) return true;
        return false;
    }
    #endregion
}