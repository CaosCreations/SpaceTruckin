#if UNITY_2021_1_OR_NEWER
// Copyright (c) Pixel Crushers. All rights reserved.

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace PixelCrushers.DialogueSystem.UIToolkit
{

    /// <summary>
    /// Manages QTEs for UIToolkitDialogueUI.
    /// </summary>
    [Serializable]
    public class UIToolkitQTEElements : AbstractUIQTEControls
    {

        [SerializeField] private UIDocument document;
        [Tooltip("Name of document's root container.")]
        [SerializeField] private string rootContainerName;
        [SerializeField] private List<string> indicatorNames;

        protected UIDocument Document => document;
        protected VisualElement RootContainer => UIToolkitDialogueUI.GetVisualElement<VisualElement>(Document, rootContainerName);

        protected virtual VisualElement GetIndicator(int index)
        {
            if (Document == null) return null;
            return Document.rootVisualElement.Q<VisualElement>(indicatorNames[index]);
        }

        public override bool areVisible
        {
            get
            {
                for (int i = 0; i < indicatorNames.Count; i++)
                {
                    if (UIToolkitDialogueUI.IsVisible(GetIndicator(i))) return true;
                }
                return false;
            }
        }

        public override void SetActive(bool value)
        {
            UIToolkitDialogueUI.SetInteractable(RootContainer, value);
            for (int i = 0; i < indicatorNames.Count; i++)
            {
                UIToolkitDialogueUI.SetDisplay(GetIndicator(i), false);
            }
        }

        public override void ShowIndicator(int index)
        {
            UIToolkitDialogueUI.SetDisplay(GetIndicator(index), true);
        }

        public override void HideIndicator(int index)
        {
            UIToolkitDialogueUI.SetDisplay(GetIndicator(index), false);
        }

    }

}
#endif
