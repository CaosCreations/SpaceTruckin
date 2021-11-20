using UnityEngine;

/// <summary>
///     Traits increase/decrease the probability of succeeding a given <see cref="Mission"/>.
///     For some Missions, they are favourable and others they are not.
/// </summary>
[CreateAssetMenu(fileName = "PilotTrait", menuName = "ScriptableObjects/PilotTrait", order = 1)]
public class PilotTrait : ScriptableObject
{
    [SerializeField] private string traitName;
    public string Name => traitName;

    [Tooltip("If the trait improves the chance of success of a mission, this is the coefficient. " +
        "Must be greater than 0.")]
    [SerializeField] private float positiveMissionProbabilityEffect;
    public float PositiveMissionProbabilityEffect => positiveMissionProbabilityEffect;

    [Tooltip("If the trait decreases the chance of success of a mission, this is the coefficient. " +
        "Must be greater than 0 and less than 1.")]
    [SerializeField] private float negativeMissionProbabilityEffect;
    public float NegativeMissionProbabilityEffect => NegativeMissionProbabilityEffect;

    [SerializeField] private Sprite traitSprite;
    public Sprite Sprite => traitSprite;


}
