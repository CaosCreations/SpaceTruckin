using UnityEngine;

[CreateAssetMenu(fileName = "RepairsMinigame", menuName = "ScriptableObjects/Minigames/RepairsMinigame", order = 1)]
public class RepairsMinigame : ScriptableObject
{
    [field: SerializeField]
    public string Name { get; set; }

    [field: SerializeField]
    public GameObject Prefab { get; set; }

    [field: SerializeField]
    public RepairsMinigameType RepairsMinigameType { get; set; }
}
