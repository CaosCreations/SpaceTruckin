using Events;
using System.Collections.Generic;
using UnityEngine;

public class NPCAnimated : MonoBehaviour
{
    [field: SerializeField]
    public Animator Animator { get; private set; }

    [SerializeField]
    private bool hasRandomIdleAnimation;

    /// <summary>
    /// Marks specific dates/animation param mappings.
    /// </summary>
    [SerializeField]
    private NPCAnimationContextByDateContainer animationContextsByDate;

    [SerializeField]
    private string defaultIdleAnimationParameter;

    public Dictionary<NPCAnimationParameterType, string> ParameterMap { get; set; } = new Dictionary<NPCAnimationParameterType, string>
    {
        { NPCAnimationParameterType.Walk, AnimationConstants.NpcWalkParameter },
        { NPCAnimationParameterType.Morning, AnimationConstants.NpcMorningParameter },
        { NPCAnimationParameterType.Evening, AnimationConstants.NpcEveningParameter }
    };

    // Used to choose a random idle animation each day 
    private List<string> idleParameterNames = new List<string>();

    private void RefreshIdleAnimation()
    {
        Animator.SetAllBoolParameters(false);
        ParameterMap[NPCAnimationParameterType.Idle] = idleParameterNames.GetRandomItem();

        NPCAnimationManager.Instance.PlayAnimation(this, NPCAnimationParameterType.Idle, true);
    }

    private bool SetAnimationParameterByDateAndPhase(TimeOfDay.Phase phase)
    {
        // Get animation parameter by current date 
        if (!animationContextsByDate.Lookup.TryGetValue(CalendarManager.CurrentDate, out var animationContext))
        {
            return false;
        }

        var parameterName = phase == TimeOfDay.Phase.Morning ? animationContext.MorningParameterName : animationContext.EveningParameterName;

        Debug.Log($"Date '{CalendarManager.CurrentDate}' uses animation param '{parameterName}' for NPC '{gameObject.name}'.");
        Animator.ResetAndSetBoolTrue(parameterName);
        return true;
    }

    private void Start()
    {
        idleParameterNames = AnimationUtils.GetIdleParameterNames(Animator);

        if (idleParameterNames.IsNullOrEmpty())
        {
            Debug.LogWarning($"No idle animation parameter keys found on {nameof(NPCAnimated)}. Will not be able to set random idle.");
        }

        ParameterMap.Add(NPCAnimationParameterType.Idle, default);

        if (string.IsNullOrWhiteSpace(defaultIdleAnimationParameter))
        {
            SetDefaultIdleParameter();
        }

        if (hasRandomIdleAnimation)
        {
            RefreshIdleAnimation();
        }

        SingletonManager.EventService.Add<OnEndOfDayEvent>(OnEndOfDayHandler);
        SingletonManager.EventService.Add<OnEveningStartEvent>(OnEveningStartHandler);
    }

    private void OnEndOfDayHandler(OnEndOfDayEvent evt)
    {
        if (hasRandomIdleAnimation)
        {
            RefreshIdleAnimation();
            return;
        }

        // If it wasn't overridden by a date-specific animation param, then use the default param 
        if (!SetAnimationParameterByDateAndPhase(TimeOfDay.Phase.Morning) && !string.IsNullOrWhiteSpace(defaultIdleAnimationParameter))
        {
            Animator.ResetAndSetBoolTrue(defaultIdleAnimationParameter);
        }
    }

    private void OnEveningStartHandler()
    {
        // If it wasn't overridden by a date-specific animation param, then use the default param 
        if (!SetAnimationParameterByDateAndPhase(TimeOfDay.Phase.Evening) && !string.IsNullOrWhiteSpace(defaultIdleAnimationParameter))
        {
            Animator.ResetAndSetBoolTrue(defaultIdleAnimationParameter);
        }
    }

    private void SetDefaultIdleParameter()
    {
        var firstTrueParam = Animator.GetFirstTrueBool();

        if (firstTrueParam == null)
        {
            Debug.LogWarning($"Unable to find a true bool param in the animator on the '{gameObject.name}' NPC. Unable to set default idle param.");
            return;
        }

        defaultIdleAnimationParameter = firstTrueParam;
    }
}
