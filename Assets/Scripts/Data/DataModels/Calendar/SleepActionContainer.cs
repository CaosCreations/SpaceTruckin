using UnityEngine;

public class SleepActionContainer : IScriptableObjectContainer<SleepAction>
{
    [field: SerializeField]
    public SleepAction[] Elements { get; set; }
}
