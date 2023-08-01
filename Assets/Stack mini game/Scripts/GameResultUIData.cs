using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Game result UI data", menuName = "ScriptableObjects/Game result UI data", order = 1)]
public class GameResultUIData : ScriptableObject
{
    [SerializeField] private string state;
    public string State => state;

    [SerializeField] private string message;

    public string Message => message;

    [SerializeField] private bool stackButtonActive;

    public bool StackButtonActive => stackButtonActive;

    [SerializeField] private bool replayButtonActive;

    public bool ReplayButtonActive => replayButtonActive;
}
