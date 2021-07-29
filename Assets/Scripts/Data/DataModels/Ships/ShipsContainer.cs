using UnityEngine;

[CreateAssetMenu(fileName = "ShipsContainer", menuName = "ScriptableObjects/ShipsContainer", order = 1)]
public class ShipsContainer : ScriptableObject, IScriptableObjectContainer<Ship>
{
    [field: SerializeField]
    public Ship[] Elements { get; set; }
}
