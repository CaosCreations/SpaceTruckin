#if UNITY_2022_1_OR_NEWER
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
        protected VisualElement RootContainer => UIToolkitUtility.GetVisualElement<VisualElement>(Document, rootContainerName);
        protected VisualElement AlertPanel => UIToolkitUtility.GetVisualElement<VisualElement>(Document, alertPanelName);

        protected TextElement alertLabel = null;
        protected TextElement AlertLabel
        {
            get
            {
                if (alertLabel == null) alertLabel = new TextElement(Document, alertLabelName);
                return alertLabel;
            }
        }

        public override bool isVisible => UIToolkitUtility.IsVisible(AlertPanel) || UIToolkitUtility.IsVisible(AlertLabel);

        public override void SetActive(bool value)
        {
            UIToolkitUtility.SetInteractable(RootContainer, value);
            UIToolkitUtility.SetDisplay(AlertPanel, value);
            UIToolkitUtility.SetDisplay(AlertLabel, value);
        }

        public override void SetMessage(string message, float duration)
        {
            if (AlertLabel != null) AlertLabel.text = message;
        }

    }

}
#endif
