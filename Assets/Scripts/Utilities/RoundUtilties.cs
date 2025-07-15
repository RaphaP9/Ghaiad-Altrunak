using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class RoundUtilties 
{
    private const float TIMED_ROUND_MINIMUM_SPAWN_INTERVAL = 0.5f;

    #region Timed Round

    //DINAMIC
    public static EnemySO GetRandomDinamicEnemyByWeight(List<ProceduralRoundEnemyGroup> proceduralRoundEnemyGroups, float weightNormalizedIncreaseFactor, float normalizedWaveElapsedTime)
    {
        float dinamicNormalizedWeightIncrease = weightNormalizedIncreaseFactor * normalizedWaveElapsedTime;

        int totalWeight = GetTotalProceduralRoundEnemyGroupsWeight(proceduralRoundEnemyGroups);
        int singularDinamicWaveEnemyWeightIncrease = Mathf.RoundToInt(totalWeight * dinamicNormalizedWeightIncrease); //Weight that every enemy will increase
        int totalDinamicWeight = totalWeight + singularDinamicWaveEnemyWeightIncrease * proceduralRoundEnemyGroups.Count; //Increase the total weight by the previous quantity times the number of wave enemies

        int randomValue = Random.Range(0, totalDinamicWeight); //Calculate the random value based on the totalDinamicWeight

        int currentWeight = 0;

        foreach (ProceduralRoundEnemyGroup proceduralRoundEnemyGroup in proceduralRoundEnemyGroups)
        {
            currentWeight += proceduralRoundEnemyGroup.weight + singularDinamicWaveEnemyWeightIncrease; //Add the singularDinamicWeight to the real weight of the enemy

            if (randomValue <= currentWeight) return proceduralRoundEnemyGroup.enemySO;
        }

        return proceduralRoundEnemyGroups[0].enemySO;
    }

    //SIMPLE
    public static EnemySO GetRandomEnemyByWeight(List<ProceduralRoundEnemyGroup> proceduralRoundEnemyGroups)
    {
        int totalWeight = GetTotalProceduralRoundEnemyGroupsWeight(proceduralRoundEnemyGroups);
        int randomValue = UnityEngine.Random.Range(0, totalWeight);

        int currentWeight = 0;

        foreach (ProceduralRoundEnemyGroup proceduralRoundEnemyGroup in proceduralRoundEnemyGroups)
        {
            currentWeight += proceduralRoundEnemyGroup.weight;

            if (randomValue <= currentWeight) return proceduralRoundEnemyGroup.enemySO;
        }

        return proceduralRoundEnemyGroups[0].enemySO;
    }

    //DINAMIC
    public static float GetTimedRoundDinamicSpawnInterval(float baseSpawnInterval, float spawnIntervalReductionFactor, float normalizedWaveElapsedTime)
    {
        //Ex. spawnIntervalReductionFactor = 0.2f , baseSpawnInterval = 5s
        //At 0 normalized elapsed wave time, spawn interval is 5s, at 1 normalized elapsed wave time (wave end), spawn interval is 5*(1-0.2) = 4. Spawn interval reduced in 20%

        float dinamicSpawnInterval = baseSpawnInterval * (1 - normalizedWaveElapsedTime * spawnIntervalReductionFactor);

        dinamicSpawnInterval = Mathf.Max(dinamicSpawnInterval, TIMED_ROUND_MINIMUM_SPAWN_INTERVAL); //Safety Reasons - Spawn time should not be less than TIMED_ROUND_MINIMUM_SPAWN_INTERVAL

        return dinamicSpawnInterval;
    }

    public static int GetTotalProceduralRoundEnemyGroupsWeight(List<ProceduralRoundEnemyGroup> proceduralRoundEnemyGroups)
    {
        int totalWeight = 0;

        foreach (ProceduralRoundEnemyGroup proceduralRoundEnemyGroup in proceduralRoundEnemyGroups)
        {
            totalWeight += proceduralRoundEnemyGroup.weight;
        }

        return totalWeight;
    }

    #endregion
}
