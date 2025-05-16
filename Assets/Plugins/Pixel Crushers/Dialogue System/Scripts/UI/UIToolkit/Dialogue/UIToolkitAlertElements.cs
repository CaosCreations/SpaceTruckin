#if UNITY_2021_1_OR_NEWER
// Copyright (c) Pixel Crushers. All rights reserved.

using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace PixelCrushers.DialogueSystem.UIToolkit
{

    /// <summary>
    /// Manages alert panel for UIToolkitDialogueUI.
    /// </summary>
    [Serializable]
    public class UIToolkitAlertElements : AbstractUIAlertControls
    {

        [SerializeField] private UIDocument document;
        [Tooltip("Name of document's root container.")]
        [SerializeField] private string rootContainerName;
        [Tooltip("Optional container panel for alert text.")]
        [SerializeField] private string alertPanelName;
        [Tooltip("Alert text.")]
        [SerializeField] private string alertLabelName;

        protected UIDocument Document => document;
        protected VisualElement RootContainer => UIToolkitDialogueUI.GetVisualElement<VisualElement>(Document, rootContainerName);
        protected VisualElement AlertPanel => UIToolkitDialogueUI.GetVisualElement<VisualElement>(Document, alertPanelName);
        protected Label AlertLabel => UIToolkitDialogueUI.GetVisualElement<Label>(Document, alertLabelName);

        public override bool isVisible => UIToolkitDialogueUI.IsVisible(AlertPanel) || UIToolkitDialogueUI.IsVisible(AlertLabel);

        public override void SetActive(bool value)
        {
            UIToolkitDialogueUI.SetInteractable(RootContainer, value);
            UIToolkitDialogueUI.SetDisplay(AlertPanel, value);
            UIToolkitDialogueUI.SetDisplay(AlertLabel, value);
        }

        public override void SetMessage(string message, float duration)
        {
            if (AlertLabel != null) AlertLabel.text = message;
        }

    }

}
#endif
