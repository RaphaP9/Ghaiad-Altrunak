using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCinematic : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private VideoCinematicSO videoCinematicSO;

    private void Update()
    {
        Test();
    }

    private void Test()
    {
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            VideoCinematicManager.Instance.StartCinematic(videoCinematicSO);
        }

        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            VideoCinematicManager.Instance.SkipCinematic();
        }
    }
}
