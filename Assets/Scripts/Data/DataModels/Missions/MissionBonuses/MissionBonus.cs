using UnityEngine;

public interface IBonus
{
    BonusExponents BonusExponents { get; set; }
}

/// <summary>
/// <para> Multiply the rewards from MissionOutcomes by the relevant value.</para>
/// <para> They're always greater than 1.</para>
/// </summary>
public struct BonusExponents
{
    public float MoneyExponent;
    public float XpExponent;
}

/// <summary>
/// Percentage increases for Mission rewards that are agreed upon in advance of the Mission.
/// </summary>
[CreateAssetMenu(fileName = "Mission", menuName = "ScriptableObjects/Mission", order = 1)]
public class MissionBonus : ScriptableObject, IBonus
{
    [field: SerializeField] public BonusExponents BonusExponents { get; set; }

    [SerializeField] private string flavourText;

    /// <summary>
    /// Some Bonuses are Pilot-specific, meaning they can only be claimed if a certain Pilot is chosen
    /// for the mission. 
    /// </summary>
    [Tooltip("Enter a value here if the bonus is only received when a particular Pilot takes the Mission.")]
    [SerializeField] private Pilot pilotRequired;

    #region Accessors
    public string FlavourText => flavourText;
    public Pilot PilotRequired => pilotRequired;
    public float MoneyExponent => BonusExponents.MoneyExponent;
    public float XpExponent => BonusExponents.XpExponent;
    #endregion
}
