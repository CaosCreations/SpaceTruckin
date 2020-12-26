using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public enum Species
{
    Human, Helicid, Myorijiin, Oshunian, HelmetGuy, Vesta
}

[CreateAssetMenu(fileName = "Pilot", menuName = "ScriptableObjects/Pilot", order = 1)]
public class Pilot : ScriptableObject
{
    [Header("Leave this blank. It is set automatically.")]
    public int id; 

    public int xp, level, missionsCompleted;
    public string pilotName, shipName, description;
    public bool hired, onMission, isAssignedToShip; 
    public Species species;
    public Ship ship; 
    public Sprite avatar;
}