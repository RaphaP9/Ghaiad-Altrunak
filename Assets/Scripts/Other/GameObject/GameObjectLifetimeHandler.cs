using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectLifetimeHandler : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField, Range(0f, 10f)] private float lifetime;

    private void Start()
    {
        StartCoroutine(LifetimeCoroutine());
    }

    private IEnumerator LifetimeCoroutine()
    {
        yield return new WaitForSeconds(lifetime);
        Destroy(gameObject);
    }
}
