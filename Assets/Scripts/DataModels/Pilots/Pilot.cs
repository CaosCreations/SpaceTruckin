using UnityEngine;
public enum Species
{
    Human, Helicid, Myorijiin, Oshunian, HelmetGuy, Vesta
}

[CreateAssetMenu(fileName = "Pilot", menuName = "ScriptableObjects/Pilot", order = 1)]
public class Pilot : ScriptableObject
{
    public class PilotSaveData
    {
        // Data to persist
        public int xp, level, missionsCompleted;
        public bool hired, onMission, isAssignedToShip;
    }

    [Header("Leave this blank. It is set automatically.")]
    public int id;

    [Header("Set in Editor")]
    public string pilotName, shipName, description;
    int hireCost;
    public Species species;
    public Ship ship; 
    public Sprite avatar;

    [Header("Data to update IN GAME")]
    public PilotSaveData pilotSaveData;
}