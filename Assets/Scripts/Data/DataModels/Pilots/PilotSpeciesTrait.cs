using UnityEngine;

[CreateAssetMenu(fileName = "PilotSpeciesTrait", menuName = "ScriptableObjects/Pilots/PilotSpeciesTrait", order = 1)]
public class PilotSpeciesTrait : PilotTrait
{
    [SerializeField] private Species species;
    public Species Species => species;

}
