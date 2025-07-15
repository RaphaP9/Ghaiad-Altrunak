using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestShopOpening : MonoBehaviour
{
    private void Update()
    {
        Test();
    }

    private void Test()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            ShopOpeningManager.Instance.OpenShop();
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            ShopOpeningManager.Instance.CloseShop();
        }
    }
}
