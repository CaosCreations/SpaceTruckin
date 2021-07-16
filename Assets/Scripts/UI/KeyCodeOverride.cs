using UnityEngine;

/// <summary>
/// A key that cannot be used anywhere outside of the menu that is currently overriding it.
/// </summary>
[System.Serializable]
public class KeyCodeOverride
{
    [field: SerializeField] 
    public KeyCode KeyCode { get; private set; }
    
    /// <summary>
    /// If true, when closing the menu this will not reset the override. 
    /// </summary>
    [field: SerializeField] 
    public bool IsPersistentOnDisable { get; private set; }

    public KeyCodeOverride(KeyCode keyCode)
    {
        KeyCode = keyCode;
    }
}
