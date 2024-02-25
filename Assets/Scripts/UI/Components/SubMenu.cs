using System.Collections.Generic;
using UnityEngine;

public class SubMenu : MonoBehaviour
{
    // Keys that will not be usable anywhere else while this menu is active
    [SerializeField] private List<KeyCodeOverride> keyCodeOverrides;

    // Set of unique overrides 
    protected HashSet<KeyCodeOverride> uniqueKeyCodeOverrides = new();

    private void OnValidate()
    {
        SetUniqueOverrides();
    }

    protected virtual void Awake()
    {
        SetUniqueOverrides();
    }

    protected void AddOverriddenKeys()
    {
        UIManager.AddOverriddenKeys(uniqueKeyCodeOverrides);
    }

    protected void RemoveOverriddenKeys()
    {
        UIManager.RemoveOverriddenKeys(uniqueKeyCodeOverrides);
    }

    protected virtual void OnEnable()
    {
        AddOverriddenKeys();
    }

    protected virtual void OnDisable()
    {
        RemoveOverriddenKeys();
    }

    private void SetUniqueOverrides()
    {
        if (keyCodeOverrides != null)
        {
            uniqueKeyCodeOverrides = keyCodeOverrides.ToHashSet();
        }
        else
        {
            Debug.LogError($"{nameof(keyCodeOverrides)} is null in {nameof(SubMenu)}. Cannot set {nameof(uniqueKeyCodeOverrides)}");
        }
    }
}
