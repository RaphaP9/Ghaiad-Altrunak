using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class RebindingUIHandler : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private List<RebindingUIGroup> rebindingUIGroups;
    [Space]
    [SerializeField] private List<Transform> transformsToEnableWhileRebinding;
    [SerializeField] private List<Transform> transformsToDisableWhileRebinding;

    private void OnEnable()
    {
        CentralizedInputSystemManager.OnRebindingStarted += CentralizedInputSystemManager_OnRebindingStarted;
        CentralizedInputSystemManager.OnRebindingCompleted += CentralizedInputSystemManager_OnRebindingCompleted;
    }

    private void OnDisable()
    {
        CentralizedInputSystemManager.OnRebindingStarted -= CentralizedInputSystemManager_OnRebindingStarted;
        CentralizedInputSystemManager.OnRebindingCompleted -= CentralizedInputSystemManager_OnRebindingCompleted;
    }

    private void Awake()
    {
        InitializeButtonsListeners();
    }

    private void Start()
    {
        SetTransforms(transformsToDisableWhileRebinding, true);
        SetTransforms(transformsToEnableWhileRebinding, false);

        UpdateBindingTexts();
    }

    private void InitializeButtonsListeners()
    {
        foreach(RebindingUIGroup rebindingUIGroup in rebindingUIGroups)
        {
            rebindingUIGroup.bindingButton.onClick.AddListener(() => RebindBinding(rebindingUIGroup));
        }
    }

    private void SetTransforms(List<Transform> transforms, bool enable)
    {
        foreach(Transform transform in transforms)
        {
            transform.gameObject.SetActive(enable);
        }
    }
    
    private void RebindBinding(RebindingUIGroup rebindingUIGroup)
    {
        CentralizedInputSystemManager.Instance.RebindBinding(rebindingUIGroup.binding);
    }

    private void UpdateBindingTexts()
    {
        foreach(RebindingUIGroup rebindingUIGroup in rebindingUIGroups)
        {
            string bindingText = MappingUtilities.MapShortBindingName(CentralizedInputSystemManager.Instance.GetBindingText(rebindingUIGroup.binding));
            rebindingUIGroup.bindingText.text = bindingText;
        }
    }

    #region Subscriptions
    private void CentralizedInputSystemManager_OnRebindingStarted(object sender, System.EventArgs e)
    {
        SetTransforms(transformsToDisableWhileRebinding, false);
        SetTransforms(transformsToEnableWhileRebinding, true);
    }

    private void CentralizedInputSystemManager_OnRebindingCompleted(object sender, System.EventArgs e)
    {
        SetTransforms(transformsToDisableWhileRebinding, true);
        SetTransforms(transformsToEnableWhileRebinding, false);

        UpdateBindingTexts();
    }
    #endregion
}

[System.Serializable]
public class RebindingUIGroup
{
    public Binding binding;
    public Button bindingButton;
    public TextMeshProUGUI bindingText;
}
