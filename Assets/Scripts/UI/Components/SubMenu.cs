using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubMenu : MonoBehaviour
{
    // Keys that this sub menu uses that will not be usable elsewhere
    // while this menu is active
    [SerializeField] private List<KeyCode> keysToOverride;
    protected HashSet<KeyCode> uniqueKeysToOverride;

    [SerializeField] private float overrideResetDelayInSeconds;

    private void Awake()
    {
        uniqueKeysToOverride = keysToOverride.ToHashSet();
    }

    public virtual void OnEnable()
    {
        UIManager.AddOrRemoveOverriddenKeys(uniqueKeysToOverride, true);
    }

    public virtual void OnDisable()
    {
        // Delay the release of overrides until the time has elapsed 
        StartCoroutine(DelayOverrideReset());

        UIManager.AddOrRemoveOverriddenKeys(uniqueKeysToOverride, false);
    }

    private IEnumerator DelayOverrideReset()
    {
        yield return new WaitForSeconds(overrideResetDelayInSeconds);
    }
}
