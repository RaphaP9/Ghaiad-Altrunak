using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestShopGeneration : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private int stageNumber;

    private void Update()
    {
        Test();
    }

    private void Test()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            List<InventoryObjectSO> generatedObjects = ShopGenerator.Instance.GenerateShopObjectsList(stageNumber);

            foreach (InventoryObjectSO obj in generatedObjects)
            {
                Debug.Log(obj);
            }
        }
    }
}
