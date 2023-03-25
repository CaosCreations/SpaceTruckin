using Events;
using UnityEngine;

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

        SetMorningParameters();
    }

    public void PlayAnimation(NPCAnimated npcAnimated, string parameterName, bool isOn)
    {
        npcAnimated.SetBoolAfterReset(parameterName, isOn);
    }

    private void SetAnimationParameterByDateAndPhase(NPC npc, TimeOfDay.Phase phase)
    {
        if (npc.Animated == null)
            return;

        var animationContext = npc.Data.GetAnimationContextByDate(CalendarManager.CurrentDate);
        var parameterName = animationContext.GetParameterNameByPhase(phase);

        if (string.IsNullOrWhiteSpace(parameterName))
        {
            Debug.LogWarning(npc + " has no animation context parameter (default nor date/phase-specific)");
            return;
        }

        PlayAnimation(npc.Animated, parameterName, true);
    }

    private void SetMorningParameters()
    {
        foreach (var npc in NPCManager.Npcs)
        {
            SetAnimationParameterByDateAndPhase(npc, TimeOfDay.Phase.Morning);
        }
    }

    private void SetEveningParameters()
    {
        foreach (var npc in NPCManager.Npcs)
        {
            SetAnimationParameterByDateAndPhase(npc, TimeOfDay.Phase.Evening);
        }
    }

    private void OnEndOfDayHandler(OnEndOfDayEvent evt)
    {
        SetMorningParameters();
    }

    private void OnEveningStartHandler()
    {
        SetEveningParameters();
    }
}
