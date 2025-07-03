// Copyright (c) Pixel Crushers. All rights reserved.

using System;
using System.Collections.Generic;
using UnityEngine;

namespace PixelCrushers.DialogueSystem
{

    /// <summary>
    /// Tells the Selector/ProximitySelector to use StandardUISelectorElements
    /// to show the current selection.
    /// </summary>
    [AddComponentMenu("")] // Use wrapper.
    public class SelectorUseStandardUIElements : MonoBehaviour
    {

        [Serializable]
        public class TagInfo
        {
            [Tooltip("Use the UI elements below for usables with this tag. Tags take precedence over layers.")]
            public string tag;
            public string defaultUseMessage;
            public StandardUISelectorElements UIElements;
        }

        public List<TagInfo> tagSpecificElements = new List<TagInfo>();

        [Serializable]
        public class LayerInfo
        {
            [Tooltip("Use the UI elements below for usables in these layers.")]
            public LayerMask layerMask;
            public string defaultUseMessage;
            public StandardUISelectorElements UIElements;
        }

        public List<LayerInfo> layerSpecificElements = new List<LayerInfo>();

        protected Selector selector = null;
        protected ProximitySelector proximitySelector = null;
        protected string defaultUseMessage = string.Empty;
        protected Usable usable = null;
        protected bool lastInRange = false;
        protected AbstractUsableUI usableUI = null;
        protected bool started = false;
        protected string originalDefaultUseMessage;
        protected bool previousUseDefaultGUI;

        protected float CurrentDistance
        {
            get
            {
                return (selector != null) ? selector.CurrentDistance : 0;
            }
        }

        protected StandardUISelectorElements m_elements = null;
        public StandardUISelectorElements elements
        {
            get { return m_elements; }
            protected set { m_elements = value; }
        }

        protected virtual void Start()
        {
            if (StandardUISelectorElements.instances.Count == 0)
            {
                if (DialogueDebug.logWarnings) Debug.LogWarning("Dialogue System: SelectorUseStandardUIElements can't find a StandardUISelectorElements component in the scene.", this);
                enabled = false;
            }
            else
            {
                started = true;
                ConnectDelegates();
                for (int i = StandardUISelectorElements.instances.Count - 1; i >= 0; i--)
                { 
                    elements = StandardUISelectorElements.instances[i];
                    if (elements != null) DeactivateControls();
                }
            }
        }

        protected virtual void OnEnable()
        {
            if (started) ConnectDelegates();
        }

        protected virtual void OnDisable()
        {
            DisconnectDelegates();
        }

        public virtual void ConnectDelegates()
        {
            DisconnectDelegates(); // Make sure we're not connecting twice.
            selector = GetComponent<Selector>();
            if (selector != null)
            {
                previousUseDefaultGUI = selector.useDefaultGUI;
                selector.useDefaultGUI = false;
                selector.Enabled += OnSelectorEnabled;
                selector.Disabled += OnSelectorDisabled;
                selector.SelectedUsableObject += OnSelectedUsable;
                selector.DeselectedUsableObject += OnDeselectedUsable;
                defaultUseMessage = selector.defaultUseMessage;
            }
            proximitySelector = GetComponent<ProximitySelector>();
            if (proximitySelector != null)
            {
                previousUseDefaultGUI = proximitySelector.useDefaultGUI;
                proximitySelector.useDefaultGUI = false;
                proximitySelector.Enabled += OnSelectorEnabled;
                proximitySelector.Disabled += OnSelectorDisabled;
                proximitySelector.SelectedUsableObject += OnSelectedUsable;
                proximitySelector.DeselectedUsableObject += OnDeselectedUsable;
                defaultUseMessage = proximitySelector.defaultUseMessage;
            }
            originalDefaultUseMessage = defaultUseMessage;
        }

        public virtual void DisconnectDelegates()
        {
            selector = GetComponent<Selector>();
            if (selector != null)
            {
                selector.useDefaultGUI = previousUseDefaultGUI;
                selector.Enabled -= OnSelectorEnabled;
                selector.Disabled -= OnSelectorDisabled;
                selector.SelectedUsableObject -= OnSelectedUsable;
                selector.DeselectedUsableObject -= OnDeselectedUsable;
            }
            proximitySelector = GetComponent<ProximitySelector>();
            if (proximitySelector != null)
            {
                proximitySelector.useDefaultGUI = previousUseDefaultGUI;
                proximitySelector.Enabled -= OnSelectorEnabled;
                proximitySelector.Disabled -= OnSelectorDisabled;
                proximitySelector.SelectedUsableObject -= OnSelectedUsable;
                proximitySelector.DeselectedUsableObject -= OnDeselectedUsable;
            }
            HideControls();
        }

        protected virtual void SetElementsForUsable(Usable usable)
        {
            // Check tag-specific UI elements:
            for (int i = 0; i < tagSpecificElements.Count; i++)
            {
                var tagInfo = tagSpecificElements[i];
                if (usable != null && usable.CompareTag(tagInfo.tag))
                {
                    defaultUseMessage = tagInfo.defaultUseMessage;
                    elements = tagInfo.UIElements ?? StandardUISelectorElements.instance;
                    return;
                }
            }

            // Check layer-specific UI elements:
            for (int i = 0; i < layerSpecificElements.Count; i++)
            {
                var layerInfo = layerSpecificElements[i];
                if (usable != null && ((1 << usable.gameObject.layer) & layerInfo.layerMask.value) != 0)
                {
                    defaultUseMessage = layerInfo.defaultUseMessage;
                    elements = layerInfo.UIElements ?? StandardUISelectorElements.instance;
                    return;
                }
            }

            // Otherwise get default elements:
            defaultUseMessage = originalDefaultUseMessage;
            if (layerSpecificElements.Count > 0 || tagSpecificElements.Count > 0)
            {
                for (int i = 0; i < StandardUISelectorElements.instances.Count; i++)
                {
                    var instance = StandardUISelectorElements.instances[i];
                    var isSpecific = (layerSpecificElements.Find(x => x.UIElements == instance) != null) ||
                        (tagSpecificElements.Find(x => x.UIElements == instance) != null);
                    if (!isSpecific)
                    {
                        elements = instance;
                        return;
                    }
                }
            }
            elements = StandardUISelectorElements.instance;
        }

        protected virtual void OnSelectedUsable(Usable usable)
        {
            this.usable = usable;
            if (usableUI != null) usableUI.Hide(); // Hide previous selection.
            usableUI = (usable != null) ? usable.GetComponentInChildren<AbstractUsableUI>() : null;
            if (usableUI != null)
            {
                usableUI.Show(GetUseMessage());
                HideControls();
            }
            else
            {
                var oldElements = elements;
                SetElementsForUsable(usable);
                if (oldElements != elements)
                {
                    var newElements = elements;
                    elements = oldElements;
                    HideControls();
                    elements = newElements;
                }
                ShowControls();
            }
            lastInRange = !IsUsableInRange();
            UpdateDisplay(!lastInRange);
        }

        protected virtual void OnDeselectedUsable(Usable usable)
        {
            if (usableUI != null)
            {
                usableUI.Hide();
                usableUI = null;
            }
            HideControls();
            this.usable = null;
        }

        protected virtual string GetUseMessage()
        {
            return DialogueManager.GetLocalizedText(string.IsNullOrEmpty(usable.overrideUseMessage) ? defaultUseMessage : usable.overrideUseMessage);
        }

        protected virtual void ShowControls()
        {
            if (usable == null || elements == null) return;
            Tools.SetGameObjectActive(elements.mainGraphic, true);
            elements.nameText.SetActive(true);
            elements.useMessageText.SetActive(true);
            elements.nameText.text = usable.GetName();
            elements.useMessageText.text = GetUseMessage();
            Tools.SetGameObjectActive(elements.reticleInRange, IsUsableInRange());
            Tools.SetGameObjectActive(elements.reticleOutOfRange, !IsUsableInRange());
            if (CanTriggerAnimations() && !string.IsNullOrEmpty(elements.animationTransitions.showTrigger))
            {
                elements.animator.ResetTrigger(elements.animationTransitions.hideTrigger);
                elements.animator.SetTrigger(elements.animationTransitions.showTrigger);
            }
        }

        protected virtual void HideControls()
        {
            if (CanTriggerAnimations() && elements != null && !string.IsNullOrEmpty(elements.animationTransitions.hideTrigger))
            {
                elements.animator.ResetTrigger(elements.animationTransitions.showTrigger);
                elements.animator.SetTrigger(elements.animationTransitions.hideTrigger);
            }
            else
            {
                DeactivateControls();
            }
        }

        protected virtual void DeactivateControls()
        {
            if (elements == null) return;
            elements.nameText.SetActive(false);
            elements.useMessageText.SetActive(false);
            Tools.SetGameObjectActive(elements.reticleInRange, false);
            Tools.SetGameObjectActive(elements.reticleOutOfRange, false);
            Tools.SetGameObjectActive(elements.mainGraphic, false);
        }

        protected virtual bool IsUsableInRange()
        {
            return (usable != null) && (CurrentDistance <= usable.maxUseDistance);
        }

        public virtual void Update()
        {
            if (usable != null)
            {
                UpdateDisplay(IsUsableInRange());
            }
        }

        protected virtual void OnSelectorEnabled()
        {
            ShowControlsOrUsableUI();
        }

        protected virtual void OnSelectorDisabled()
        {
            HideControls();
        }

        public virtual void OnConversationStart(Transform actor)
        {
            HideControls();
        }

        public virtual void OnConversationEnd(Transform actor)
        {
            ShowControlsOrUsableUI();
        }

        protected virtual void ShowControlsOrUsableUI()
        { 
            if (usableUI != null)
            {
                usableUI.Show(GetUseMessage());
            }
            else
            {
                ShowControls();
            }
        }

        protected virtual void UpdateDisplay(bool inRange)
        {
            if ((usable != null) && (inRange != lastInRange))
            {
                lastInRange = inRange;
                if (usableUI != null)
                {
                    usableUI.UpdateDisplay(inRange);
                }
                else
                {
                    UpdateText(inRange);
                    UpdateReticle(inRange);
                }
            }
        }

        protected virtual void UpdateText(bool inRange)
        {
            if (elements == null) return;
            if (elements.useRangeColors)
            {
                var color = inRange ? elements.inRangeColor : elements.outOfRangeColor;
                if (elements.nameText != null) elements.nameText.color = color;
                if (elements.useMessageText != null) elements.useMessageText.color = color;
            }
        }

        protected virtual void UpdateReticle(bool inRange)
        {
            if (elements == null) return;
            Tools.SetGameObjectActive(elements.reticleInRange, inRange);
            Tools.SetGameObjectActive(elements.reticleOutOfRange, !inRange);
        }

        protected virtual bool CanTriggerAnimations()
        {
            return (elements != null) && (elements.animator != null) && (elements.animationTransitions != null);
        }

    }

}
