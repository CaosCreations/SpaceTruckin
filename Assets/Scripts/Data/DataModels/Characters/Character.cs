using UnityEngine;

[CreateAssetMenu(fileName = "Character", menuName = "ScriptableObjects/Character", order = 1)]
public class Character : ScriptableObject
{
    [field: SerializeField]
    public string Name { get; set; }

    [field: SerializeField]
    public Sprite PreviewSprite { get; set; }

    [field: SerializeField]
    public GameObject Prefab { get; set; }

    [field: SerializeField]
    public AnimatorSettings AnimatorSettings { get; set; }
}
