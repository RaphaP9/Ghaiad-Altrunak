using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAddRemoveObject : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private ObjectSO objectSO;

    private void Update()
    {
        Test();
    }

    private void Test()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            ObjectsInventoryManager.Instance.AddObjectToInventory(objectSO);
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            ObjectsInventoryManager.Instance.RemoveObjectFromInventory(objectSO);
        }
    }
}
