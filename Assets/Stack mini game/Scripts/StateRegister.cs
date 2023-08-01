using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "State register", menuName = "ScriptableObjects/Stack mini game state register", order = 1)]
public class StateRegister : ScriptableObject
{
    [SerializeField] private string[] states;

    public string[] States => states;
}
