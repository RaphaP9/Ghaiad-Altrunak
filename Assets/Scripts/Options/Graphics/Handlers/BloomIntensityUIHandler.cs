using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloomIntensityUIHandler : PostProcessingLinearValueUIHandler
{
    private void OnEnable()
    {
        BloomIntensityManager.OnBloomIntensityManagerInitialized += BloomIntensityManager_OnBloomIntensityManagerInitialized;
        BloomIntensityManager.OnBloomIntensityChanged += BloomIntensityManager_OnBloomIntensityChanged;
    }

    private void OnDisable()
    {
        BloomIntensityManager.OnBloomIntensityManagerInitialized -= BloomIntensityManager_OnBloomIntensityManagerInitialized;
        BloomIntensityManager.OnBloomIntensityChanged -= BloomIntensityManager_OnBloomIntensityChanged;
    }

    protected override PostProcessingLinearValueManager GetPostProcessingManager() => BloomIntensityManager.Instance;

    private void BloomIntensityManager_OnBloomIntensityManagerInitialized(object sender, System.EventArgs e)
    {
        UpdateVisual();
    }

    private void BloomIntensityManager_OnBloomIntensityChanged(object sender, BloomIntensityManager.OnIntensityChangedEventArgs e)
    {
        UpdateVisual();
    }
}
