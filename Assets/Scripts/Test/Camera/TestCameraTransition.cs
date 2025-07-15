using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCameraTransition : MonoBehaviour
{
    private void Update()
    {
        TestStartTransition();
        TestEndTransition();
    }

    private void TestEndTransition()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            GameLogManager.Instance.Log("Start");
        }
    }

    private void TestStartTransition()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            GameLogManager.Instance.Log("End");
        }
    }
}
