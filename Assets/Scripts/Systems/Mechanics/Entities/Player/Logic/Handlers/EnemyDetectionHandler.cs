using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class EnemyDetectionHandler : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField,Range(1f,10f)] private float detectionRange;
    [SerializeField] private LayerMask enemiesLayerMask;

    [Header("Lists")]
    [SerializeField] private List<Transform> detectedEnemies;

    [Header("Debug")]
    [SerializeField] private bool debug;    

    public List<Transform> Enemies => detectedEnemies;

    private void Update()
    {
        HandleEnemyDetection();
    }

    private void HandleEnemyDetection()
    {
        Collider2D[] detectedEnemiesColliders = Physics2D.OverlapCircleAll(GeneralUtilities.TransformPositionVector2(transform), detectionRange, enemiesLayerMask);

        List<Transform> detectedEnemies = GeneralUtilities.GetTransformsByColliders(detectedEnemiesColliders);

        //Get New and Old Enemies using Linq
        List<Transform> newEnemiesDetected = detectedEnemies.Except(detectedEnemies).ToList();
        List<Transform> oldEnemiesDetected = detectedEnemies.Except(detectedEnemies).ToList();

        //Add New Enemies Retected
        foreach (Transform newEnemyDetected in newEnemiesDetected)
        {
            this.detectedEnemies.Add(newEnemyDetected);
        }

        //Remove Old Enemies Detected
        foreach (Transform oldEnemyDetected in oldEnemiesDetected)
        {
            this.detectedEnemies.Remove(oldEnemyDetected);
        }

        foreach(Transform enemy in this.detectedEnemies)
        {
            if (enemy == null) Debug.Log("Null Detected");
        }

        this.detectedEnemies.RemoveAll(t => t == null); //Remove null transforms 
    }

    private void OnDrawGizmos()
    {
        if(!debug) return;

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}
