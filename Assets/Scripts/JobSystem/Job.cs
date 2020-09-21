using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Job", menuName = "ScriptableObjects/Job", order = 1)]
public class Job : ScriptableObject
{
    public string title;
    public int reward;
    public float duration;

    // Used for job container positioning 
    public Vector2 startingPosition; 
}
