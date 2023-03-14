using Events;
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
    private List<string> idleParameterNames = new List<string>();

    private void RefreshIdleAnimation()
    {
        Animator.SetAllBoolParameters(false);
        ParameterMap[NPCAnimationParameterType.Idle] = idleParameterNames.GetRandomItem();

        NPCAnimationManager.Instance.PlayAnimation(this, NPCAnimationParameterType.Idle, true);
    }

    private void OnEndOfDayHandler(OnEndOfDayEvent evt)
    {
        RefreshIdleAnimation();
    }

    private void Start()
    {
        idleParameterNames = AnimationUtils.GetIdleParameterNames(Animator);

        if (idleParameterNames.IsNullOrEmpty())
        {
            Debug.LogError($"No idle animation parameter keys found on {nameof(NPCAnimated)}");
        }

        ParameterMap.Add(NPCAnimationParameterType.Idle, default);

        RefreshIdleAnimation();
        SingletonManager.EventService.Add<OnEndOfDayEvent>(RefreshIdleAnimation);
    }
}
