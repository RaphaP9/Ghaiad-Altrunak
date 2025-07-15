using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlayerTeleport : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Vector2 teleportPosition;

    private void Update()
    {
        Test();
    }

    private void Test()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            PlayerTeleporterManager.Instance.TeleportPlayerToPosition(teleportPosition, true);
        }
    }
}
