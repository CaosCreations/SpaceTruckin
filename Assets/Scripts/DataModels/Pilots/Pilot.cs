using UnityEngine;
public enum Species
{
    Human, Helicid, Myorijiin, Oshunian, HelmetGuy, Vesta
}

[CreateAssetMenu(fileName = "Pilot", menuName = "ScriptableObjects/Pilot", order = 1)]
public class Pilot : ScriptableObject
{
    [Header("Leave this blank. It is set automatically.")]
    public int id;

    public int xp, level, missionsCompleted, hireCost;
    public string pilotName, shipName, description;
    public bool isHired, isOnMission, isAssignedToShip; 
    public Species species;
    public Ship ship; 
    public Sprite avatar;
}