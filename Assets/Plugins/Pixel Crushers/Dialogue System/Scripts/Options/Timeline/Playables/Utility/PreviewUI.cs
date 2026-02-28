#if USE_TIMELINE
#if UNITY_2017_1_OR_NEWER
// Copyright (c) Pixel Crushers. All rights reserved.

using UnityEngine;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif

namespace PixelCrushers.DialogueSystem
{

    /// <summary>
    /// This MonoBehaviour is used internally by the Dialogue System's
    /// playables to show an editor representation of activity that can
    /// only be accurately viewed at runtime.
    /// </summary>
    [AddComponentMenu("")] // No menu item. Only used internally.
    [ExecuteInEditMode]
    public class PreviewUI : MonoBehaviour
    {

#if UNITY_EDITOR

        private static PreviewUI instance = null;
        private static PreviewUI Instance
        {
            get
            {
                if (isQuittingOrChangingPlayMode) return null;
                if (instance == null)
                {
                    instance = GameObjectUtility.FindFirstObjectByType<PreviewUI>();
                    if (instance == null) instance = CreateInstance();
                }
                return instance;
            }
        }
        private static bool isQuittingOrChangingPlayMode;

        private string message;
        private bool computedRect;
        private Rect rect;
        private GUIStyle guiStyle = null;

        [InitializeOnLoadMethod]
        private static void InitializeOnLoad()
        {
            EditorSceneManager.sceneSaving += OnSceneSaving;
            EditorSceneManager.sceneUnloaded += OnSceneUnloaded;
            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
            UnityEditor.Compilation.CompilationPipeline.compilationStarted += OnCompilationStarted;
        }

        private static void OnSceneSaving(Scene scene, string path)
        {
            DestroyInstance();
        }

        private static void OnSceneUnloaded(Scene scene)
        {
            DestroyInstance();
        }

        private static void OnCompilationStarted(object obj)
        {
            DestroyInstance();
        }

        private static void OnPlayModeStateChanged(PlayModeStateChange change)
        {
            DestroyInstance();
            isQuittingOrChangingPlayMode = true;
        }

        [UnityEditor.Callbacks.DidReloadScripts]
        private static void DidReloadScripts()
        {
            DestroyInstance();
        }

        private static PreviewUI CreateInstance()
        {
            var go = new GameObject("DialogueSystemPreviewUI");
            go.tag = "EditorOnly";
            //go.hideFlags = HideFlags.DontSave;
            return go.AddComponent<PreviewUI>();
        }

        private static void DestroyInstance()
        {
            if (instance != null) DestroyImmediate(instance.gameObject);
            instance = null;
        }

        private void OnApplicationQuit()
        {
            isQuittingOrChangingPlayMode = true;
        }

        private void OnGUI()
        {
            if (string.IsNullOrEmpty(message)) return;
            if (guiStyle == null)
            {
                guiStyle = new GUIStyle(GUI.skin.label);
                guiStyle.fontSize = 26;
                guiStyle.fontStyle = FontStyle.Bold;
                guiStyle.alignment = TextAnchor.MiddleCenter;
            }
            if (!computedRect)
            {
                computedRect = true;
                var size = guiStyle.CalcSize(new GUIContent(message));
                rect = new Rect((Screen.width - size.x) / 2, Screen.height - (2 * size.y), size.x, size.y);
            }
            GUI.Label(rect, message, guiStyle);
        }

        public static void ShowMessage(string message)
        {
            if (Instance == null) return;
            Instance.message = message;
            Instance.computedRect = false;
            Debug.Log(message);
        }

        public static void HideMessage()
        {
            if (Instance == null) return;
            Instance.message = string.Empty;
        }

#else // Builds (not editor)

        public static void ShowMessage(string message) {}

        public static void HideMessage() {}

#endif


    }
}
#endif
#endif
