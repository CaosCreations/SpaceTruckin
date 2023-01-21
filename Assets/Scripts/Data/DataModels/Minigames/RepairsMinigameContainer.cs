using UnityEngine;

[CreateAssetMenu(fileName = "RepairsMinigameContainer", menuName = "ScriptableObjects/Minigames/RepairsMinigameContainer", order = 1)]
public class RepairsMinigameContainer : ScriptableObject, IScriptableObjectContainer<RepairsMinigame>
{
    [field: SerializeField]
    public RepairsMinigame[] Elements { get; set; }
}