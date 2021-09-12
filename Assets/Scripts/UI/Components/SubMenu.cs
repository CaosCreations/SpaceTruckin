using System.Collections.Generic;
using UnityEngine;

public class SubMenu : MonoBehaviour
{
    // Keys that will not be usable anywhere else while this menu is active
    [SerializeField] private List<KeyCodeOverride> keyCodeOverrides;

    // Set of unique overrides 
    protected HashSet<KeyCodeOverride> uniqueKeyCodeOverrides;

    private void OnValidate()
    {
        SetUniqueOverrides();
    }

    private void Awake()
    {
        SetUniqueOverrides();
    }

    public virtual void OnEnable()
    {
        UIManager.AddOverriddenKeys(uniqueKeyCodeOverrides);
    }

    public virtual void OnDisable()
    {
        UIManager.RemoveOverriddenKeys(uniqueKeyCodeOverrides);
    }

    private void SetUniqueOverrides()
    {
        if (keyCodeOverrides != null)
        {
            uniqueKeyCodeOverrides = keyCodeOverrides.ToHashSet();
        }
        else
        {
            Debug.LogError(
                $"{nameof(keyCodeOverrides)} is null in {nameof(SubMenu)}. Cannot set {nameof(uniqueKeyCodeOverrides)}");
        }
    }
}
