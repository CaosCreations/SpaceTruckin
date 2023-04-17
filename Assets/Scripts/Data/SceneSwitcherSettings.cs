using System;
using UnityEngine;

[CreateAssetMenu(fileName = "SceneSwitcherSettings", menuName = "ScriptableObjects/Editor/SceneSwitcherSettings", order = 1)]
public class SceneSwitcherSettings : ScriptableObject
{
    [field: SerializeField]
    public SceneSwitcherSettingsEntry[] Entries { get; private set; }
}

[Serializable]
public class SceneSwitcherSettingsEntry
{
    [field: SerializeField]
    public string SceneName { get; private set; }

    [field: SerializeField]
    public string FilePath { get; private set; }
}