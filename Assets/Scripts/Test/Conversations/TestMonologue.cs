using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMonologue : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private MonologueSO monologueSO;

    private void Update()
    {
        Test();
    }

    private void Test()
    {
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            MonologueManager.Instance.StartMonologue(monologueSO);
        }

        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            MonologueManager.Instance.EndSentence();
        }

        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            MonologueManager.Instance.EndMonologue();
        }
    }
}
