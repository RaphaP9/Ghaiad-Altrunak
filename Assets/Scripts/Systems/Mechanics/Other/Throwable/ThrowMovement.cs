using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowMovement : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] protected Rigidbody2D _rigidbody2D;

    [Header("FreeFall Settings")]
    [SerializeField] protected LayerMask terrainLayerMask;
    [SerializeField, Range(2f, 10f)] protected float minImpulse;
    [SerializeField, Range(2f, 10f)] protected float maxImpulse;
    [SerializeField, Range(-360f, 360)] protected float minVelAngle;
    [SerializeField, Range(-360f, 360)] protected float maxVelAngle;
    [Space]
    [SerializeField, Range(0.2f, 2.5f)] protected float minTimeToStop;
    [SerializeField, Range(0.2f, 2.5f)] protected float maxTimeToStop;

    [Header("Runtiime Filled")]
    [SerializeField] protected float chosenAngle;
    [SerializeField] protected Vector2 chosenDirection;

    protected void Start()
    {
        ChooseRandomDirection();
        ThrowInChosenDirection();
        StartCoroutine(StopInTimeCoroutine());
    }

    protected void ThrowInChosenDirection()
    {
        float impulse = UnityEngine.Random.Range(minImpulse, maxImpulse);
        _rigidbody2D.AddForce(impulse * chosenDirection, ForceMode2D.Impulse);
    }

    protected void ChooseRandomDirection()
    {
        chosenAngle = UnityEngine.Random.Range(minVelAngle, maxVelAngle);
        chosenDirection = GeneralUtilities.AngleDegreesToVector2(chosenAngle);
    }

    protected IEnumerator StopInTimeCoroutine()
    {
        float stopTime = UnityEngine.Random.Range(minTimeToStop, maxTimeToStop);
        yield return new WaitForSeconds(stopTime);
        StopMovement();
    }

    protected void StopMovement()
    {
        _rigidbody2D.gravityScale = 0f;
        _rigidbody2D.velocity = Vector2.zero;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!GeneralUtilities.CheckGameObjectInLayerMask(collision.gameObject, terrainLayerMask)) return;
        StopMovement();
    }
}
