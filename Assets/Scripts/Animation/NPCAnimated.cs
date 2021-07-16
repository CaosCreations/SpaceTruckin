using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCAnimated : MonoBehaviour
{
    [SerializeField] public Animator Animator { get; set; }

    public Dictionary<NPCAnimationParameterType, string> ParameterMap { get; set; }
}
