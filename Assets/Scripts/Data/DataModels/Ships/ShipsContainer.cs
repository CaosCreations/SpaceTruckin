using UnityEngine;

[CreateAssetMenu(fileName = "ShipsContainer", menuName = "ScriptableObjects/ShipsContainer", order = 1)]
public class ShipsContainer : ScriptableObject
{
    public Ship[] ships;
}
