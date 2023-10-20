using System;
using UnityEngine;

[Serializable]
public class CutsceneAnimatorSettings
{
	[field: SerializeField]
	public Animator Animator { get; private set; }

	[field: SerializeField] 
	public string StateOnEnd { get; private set; }
}
