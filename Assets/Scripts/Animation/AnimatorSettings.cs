using System;
using UnityEditor.Animations;
using UnityEngine;

[Serializable]
public class AnimatorSettings
{
    [field: SerializeField]
    public AnimatorController AnimatorController { get; set; }

    [field: SerializeField]
    public Avatar Avatar { get; set; }
}