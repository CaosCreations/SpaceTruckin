using UnityEngine;

[CreateAssetMenu(fileName = "SleepActionContainer", menuName = "ScriptableObjects/SleepActionContainer", order = 1)]
public class SleepActionContainer : ScriptableObject, IScriptableObjectContainer<SleepAction>
{
    [field: SerializeField]
    public SleepAction[] Elements { get; set; }
}
