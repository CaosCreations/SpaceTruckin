using UnityEngine;

[CreateAssetMenu(fileName = "SleepAction", menuName = "ScriptableObjects/SleepAction", order = 1)]
public class SleepAction : ScriptableObject
{
    public enum Phase
    {
        Sleep, Wake
    }

    /// <summary>
    /// Friendly name.
    /// </summary>
    [field: SerializeField]
    public string Name { get; private set; }

    [field: SerializeField]
    public Date Date { get; private set; }

    /// <summary>
    /// The cutscene to play when action executes.
    /// </summary>
    [field: SerializeField]
    public Cutscene Cutscene { get; private set; }

    [field: SerializeField]
    public Phase SleepPhase { get; private set; }

    public void Execute()
    {
        if (Cutscene == null)
        {
            Debug.LogWarning("No cutscene attached to " + ToString());
            return;
        }

        TimelineManager.PlayCutscene(Cutscene);
    }

    public override string ToString()
    {
        return $"{nameof(SleepAction)} with name '{Name}'";
    }

    private void OnValidate()
    {
        Date = Date.Validate();
    }
}
