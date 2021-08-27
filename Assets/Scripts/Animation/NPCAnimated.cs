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
    private List<string> idleAnimationKeys = new List<string>();

    private void RefreshIdleAnimation()
    {
        Animator.SetAllBoolParameters(false);
        ParameterMap[NPCAnimationParameterType.Idle] = idleAnimationKeys.GetRandomItem();

        NPCAnimationManager.Instance.PlayAnimation(this, NPCAnimationParameterType.Idle, true);
    }

    private void Start()
    {
        idleAnimationKeys = AnimationUtils.GetIdleAnimationKeys(Animator);
        ParameterMap.Add(NPCAnimationParameterType.Idle, default);

        RefreshIdleAnimation();
        CalendarManager.OnEndOfDay += RefreshIdleAnimation;
    }
}
