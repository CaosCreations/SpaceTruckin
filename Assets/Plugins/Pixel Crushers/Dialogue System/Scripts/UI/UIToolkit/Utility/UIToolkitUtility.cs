#if UNITY_2022_1_OR_NEWER
// Copyright (c) Pixel Crushers. All rights reserved.

using UnityEngine.UIElements;

namespace PixelCrushers.DialogueSystem.UIToolkit
{

    /// <summary>
    /// Utility methods for working with UI Toolkit.
    /// </summary>
    public static class UIToolkitUtility
    {

        public static void SetDisplay(VisualElement visualElement, bool value, bool setFocus = false)
        {
            if (visualElement == null) return;
            visualElement.style.display = value ? DisplayStyle.Flex : DisplayStyle.None;
            if (setFocus) visualElement.Focus();
        }

        public static void SetDisplay(TextElement textElement, bool value, bool setFocus = false)
        {
            if (textElement == null) return;
            textElement.SetDisplay(value, setFocus);
        }

        public static bool IsVisible(VisualElement visualElement)
        {
            if (visualElement == null) return false;
            return visualElement.style.display != DisplayStyle.None;

        }

        public static bool IsVisible(TextElement textElement)
        {
            if (textElement == null) return false;
            return textElement.IsVisible;
        }

        public static T GetVisualElement<T>(UIDocument document, string visualElementName) where T : VisualElement
        {
            if (document == null || document.rootVisualElement == null) return null;
            return document.rootVisualElement.Q<T>(visualElementName);
        }

        public static void SetInteractable(VisualElement rootVisualElement, bool value)
        {
            if (rootVisualElement == null) return;
            rootVisualElement.pickingMode = value ? PickingMode.Position : PickingMode.Ignore;
        }

    }

}
#endif
