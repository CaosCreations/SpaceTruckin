using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AsteroidOutcome", menuName = "ScriptableObjects/Missions/Outcomes/AsteroidOutcome", order = 1)]
public class AsteroidOutcome : MissionOutcome
{
    public float probability;
    public float shipDamage;

    public override void Process()
    {
        // do ship damage
    }
}
