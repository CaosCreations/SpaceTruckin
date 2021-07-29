using UnityEngine;

/// <summary>
/// Percentage increases for Mission rewards that are agreed upon in advance of the Mission.
/// </summary>
[CreateAssetMenu(fileName = "MissionBonus", menuName = "ScriptableObjects/Missions/Bonuses/MissionBonus", order = 1)]
public class MissionBonus : ScriptableObject, IBonus
{
    [field: SerializeField] public BonusExponents BonusExponents { get; set; }

    [SerializeField] private string flavourText;

    /// <summary>
    /// Some Bonuses are Pilot-specific, meaning they can only be claimed if a certain Pilot is chosen
    /// for the mission. 
    /// </summary>
    [Tooltip("Enter a value here if the bonus is only received when a particular Pilot takes the Mission.")]
    [SerializeField] private Pilot requiredPilot;

    public bool AreCriteriaMet(ScheduledMission scheduled)
    {
        // If there is a Pilot requirement, the assigned Pilot must match the required Pilot 
        return requiredPilot == null || requiredPilot == scheduled.Pilot;
    }

    #region Accessors
    public string FlavourText => flavourText;
    public Pilot RequiredPilot { get => requiredPilot; set => requiredPilot = value; }
    public float MoneyExponent => BonusExponents.MoneyExponent;
    public float XpExponent => BonusExponents.XpExponent;
    #endregion
}
