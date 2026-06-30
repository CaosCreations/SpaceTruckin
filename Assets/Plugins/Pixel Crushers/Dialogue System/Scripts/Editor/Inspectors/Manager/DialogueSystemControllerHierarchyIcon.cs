// Copyright (c) Pixel Crushers. All rights reserved.

using UnityEngine;
using UnityEditor;

namespace PixelCrushers.DialogueSystem
{

    /// <summary>
    /// Draws the Dialogue Manager icon in the Hierarchy view.
    /// Note: New Hierarchy window mode in Unity 6.3+ currently doesn't support 
    /// EditorApplication.hierarchyWindowItemOnGUI so it won't show an icon.
    /// </summary>
    [InitializeOnLoad]
    public class DialogueSystemControllerHierarchyIcon
    {

        private const string IconFilename = "Assets/Gizmos/DialogueDatabase Icon.png";

        private static Texture2D icon = null;
        private static float minWidth = -1;

        static DialogueSystemControllerHierarchyIcon()
        {
            icon = AssetDatabase.LoadAssetAtPath<Texture2D>(IconFilename);
            if (icon == null) return;
#if UNITY_6000_4_OR_NEWER
            EditorApplication.hierarchyWindowItemByEntityIdOnGUI -= HierarchyWindowItemByEntityIdOnGUI;
            EditorApplication.hierarchyWindowItemByEntityIdOnGUI += HierarchyWindowItemByEntityIdOnGUI;
#else
            EditorApplication.hierarchyWindowItemOnGUI -= HierarchyWindowItemOnGUI;
            EditorApplication.hierarchyWindowItemOnGUI += HierarchyWindowItemOnGUI;
#endif
            EditorApplication.RepaintHierarchyWindow();
        }

#if UNITY_6000_4_OR_NEWER
        private static void HierarchyWindowItemByEntityIdOnGUI(EntityId entityId, Rect selectionRect)
        {
            ShowHierarchyIcon(new EntityIdWrapper(entityId), selectionRect);
        }
#else
        public static void HierarchyWindowItemOnGUI(int instanceID, Rect selectionRect)
        {
            ShowHierarchyIcon(new EntityIdWrapper(instanceID), selectionRect);
        }
#endif

        private static void ShowHierarchyIcon(EntityIdWrapper entityIdWrapper, Rect selectionRect)
        {
            GameObject dialogueSystemControllerGameObject = MoreEditorUtility.InstanceIDToObject(entityIdWrapper) as GameObject;
            var dialogueSystemController = (dialogueSystemControllerGameObject != null) ? dialogueSystemControllerGameObject.GetComponent<DialogueSystemController>() : null;
            if (dialogueSystemController != null && icon != null)
            {
                if (minWidth <= 0)
                {
                    var size = GUI.skin.label.CalcSize(new GUIContent(dialogueSystemControllerGameObject.name));
                    minWidth = 16f + size.x + icon.width;
                }
                if (selectionRect.width > minWidth)
                {
                    Rect rect = new Rect(selectionRect.width - 5f, selectionRect.y, selectionRect.width, selectionRect.height);
                    GUI.Label(rect, icon);
                }
            }
        }

    }

}
