using System.Collections.Generic;
using UnityEngine;

public class NPCAnimated : MonoBehaviour
{
    [field: SerializeField]
    public Animator Animator { get; private set; }

    public Dictionary<NPCAnimationParameterType, string> ParameterMap { get; set; } = new Dictionary<NPCAnimationParameterType, string>
    {
        { NPCAnimationParameterType.Walk, AnimationConstants.NpcWalkParameter }
    };

    // Used to choose a random idle animation each day 
    private string CurrentIdleKey { get; set; }
    private List<string> idleAnimationKeys = new List<string>();

    private void RandomiseIdleKey() => CurrentIdleKey = idleAnimationKeys.GetRandomItem();

    private void Awake()
    {
        idleAnimationKeys = AnimationUtils.GetIdleAnimationKeys(Animator);
        CalendarManager.OnEndOfDay += RandomiseIdleKey;

        ParameterMap.Add(NPCAnimationParameterType.Idle, CurrentIdleKey);
    }
}
