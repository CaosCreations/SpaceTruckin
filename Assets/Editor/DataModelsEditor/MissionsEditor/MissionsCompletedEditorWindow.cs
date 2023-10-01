using Events;
using UnityEditor;

public class MissionsCompletedEditor : DataModelEditorWindow<Mission>
{
    [MenuItem("Space Truckin/Missions/Missions Completed Select")]
    public static void ShowWindow()
    {
        GetWindow<MissionsCompletedEditor>("Missions Completed Select");
    }

    private void OnEnable()
    {
#if UNITY_EDITOR
        EditorApplication.playModeStateChanged += OnPlayModeStateChangedHandler;
#endif
        if (SingletonManager.Instance != null)
        {
            SingletonManager.EventService.Add<OnMissionCompletedEvent>(OnMissionCompletedHandler);
            RefreshData(MissionsManager.GetMissions());
        }
    }

    private void OnDisable()
    {
#if UNITY_EDITOR
        EditorApplication.playModeStateChanged -= OnPlayModeStateChangedHandler;
#endif
        if (SingletonManager.Instance != null)
        {
            SingletonManager.EventService.Remove<OnMissionCompletedEvent>(OnMissionCompletedHandler);
        }
        dataModels = null;
    }

    private void OnPlayModeStateChangedHandler(PlayModeStateChange state)
    {
        if (state == PlayModeStateChange.EnteredPlayMode)
        {
            RefreshData(MissionsManager.GetMissions());
        }
    }

    private void OnMissionCompletedHandler(OnMissionCompletedEvent evt)
    {
        RefreshData(MissionsManager.GetMissions());
    }
}
