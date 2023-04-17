using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public class SceneSwitcher : EditorWindow
{
    private int selectedSceneIndex = 0;

    private readonly List<(string name, string path)> sceneNamePathMap = new();

    [MenuItem("Space Truckin/Scene Switcher")]
    public static void ShowWindow()
    {
        GetWindow<SceneSwitcher>("Scene Switcher");
    }

    void OnGUI()
    {
        GUILayout.Label("Select Scene to Switch to:", EditorStyles.boldLabel);

        // Populate the dropdown list with scenes from a ScriptableObject
        var settings = AssetDatabase.LoadAssetAtPath("Assets/Editor/SceneSwitcherSettings.asset", typeof(SceneSwitcherSettings)) as SceneSwitcherSettings;
        if (settings == null)
        {
            GUILayout.Label("No SceneListScriptableObject found.");
            return;
        }

        sceneNamePathMap.Clear();
        foreach (var entry in settings.Entries)
        {
            sceneNamePathMap.Add((entry.SceneName, entry.FilePath));
        }

        selectedSceneIndex = EditorGUILayout.Popup(selectedSceneIndex, sceneNamePathMap.Select(map => map.name).ToArray());

        if (GUILayout.Button("Switch to Scene"))
        {
            // Load the selected scene
            var selected = sceneNamePathMap[selectedSceneIndex];
            EditorSceneManager.OpenScene(selected.path);
        }
    }
}
