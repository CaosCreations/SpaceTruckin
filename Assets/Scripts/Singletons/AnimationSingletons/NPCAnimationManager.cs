using Events;

public class NPCAnimationManager : AnimationManager<NPCAnimationParameterType>
{
    public static NPCAnimationManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        SingletonManager.EventService.Add<OnEveningStartEvent>(OnEveningStartHandler);
        SingletonManager.EventService.Add<OnEndOfDayEvent>(OnEndOfDayHandler);

        UpdateAnimatorForMorning();
    }

    private void UpdateAnimationByDateAndPhase(NPC npc, TimeOfDay.Phase phase)
    {
        if (npc.Animator == null
            || npc.Data == null
            || npc.Data.AnimationContextByDateContainer == null
            || npc.Data.AnimationContextByDateContainer.Lookup == null)
        {
            return;
        }

        var animationContext = npc.Data.GetAnimationContextByDate(CalendarManager.CurrentDate);
        var parameterName = animationContext.GetParameterNameByPhase(phase);

        if (!string.IsNullOrWhiteSpace(parameterName))
        {
            npc.Animator.SetBoolAfterReset(parameterName, true);
        }

        var stateName = animationContext.GetStateNameByPhase(phase);

        if (!string.IsNullOrWhiteSpace(stateName))
        {
            npc.Animator.Play(stateName);
        }
    }

    private void UpdateAnimatorForMorning()
    {
        foreach (var npc in NPCManager.Npcs)
        {
            UpdateAnimationByDateAndPhase(npc, TimeOfDay.Phase.Morning);
        }
    }

    private void UpdateAnimatorForEvening()
    {
        foreach (var npc in NPCManager.Npcs)
        {
            UpdateAnimationByDateAndPhase(npc, TimeOfDay.Phase.Evening);
        }
    }

    private void OnEndOfDayHandler(OnEndOfDayEvent evt)
    {
        UpdateAnimatorForMorning();
    }

    private void OnEveningStartHandler()
    {
        UpdateAnimatorForEvening();
    }
}
