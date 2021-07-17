using System.Collections.Generic;
using UnityEngine;

public class NPCAnimated : MonoBehaviour
{
    [field: SerializeField]
    public Animator Animator { get; private set; }

    public Dictionary<NPCAnimationParameterType, string> ParameterMap { get; set; }
}
