// Copyright (c) Pixel Crushers. All rights reserved.

using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
#if USE_NEW_INPUT
using UnityEngine.InputSystem;
#endif

namespace PixelCrushers
{

    /// <summary>
    /// This script adds a key or button trigger to a Unity UI Selectable.
    /// </summary>
    [AddComponentMenu("")] // Use wrapper.
    [RequireComponent(typeof(UnityEngine.UI.Selectable))]
    public class UIButtonKeyTrigger : MonoBehaviour, IEventSystemUser
    {

        [Tooltip("Trigger the selectable when this key is pressed.")]
        public KeyCode key = KeyCode.None;

        [Tooltip("Trigger the selectable when this input button is pressed.")]
        public string buttonName = string.Empty;

#if USE_NEW_INPUT
        [Tooltip("Trigger the selectable when this input action is performed.")]
        public InputActionReference inputAction;

        [Tooltip("Disable input action after button is clicked.")]
        public bool disableInputActionAfterClick = false;
#endif

        [Tooltip("Trigger if any key, input button, or mouse button is pressed.")]
        public bool anyKeyOrButton = false;

        [Tooltip("Ignore trigger key/button if UI button is being clicked Event System's Submit input. Prevents unintentional double clicks. For this checkbox to work, you must set the Input Device Manager component's Submit input to the same inputs as the EventSystem's Submit.")]
        public bool skipIfBeingClickedBySubmit = true;

        [Tooltip("Visually show UI Button in pressed state when triggered.")]
        public bool simulateButtonClick = true;

        [Tooltip("Show pressed state for this duration in seconds.")]
        public float simulateButtonDownDuration = 0.1f;

        private UnityEngine.UI.Selectable m_selectable;
        protected UnityEngine.UI.Selectable selectable { get { return m_selectable; } set { m_selectable = value; } }

        private UnityEngine.EventSystems.EventSystem m_eventSystem = null;
        public UnityEngine.EventSystems.EventSystem eventSystem
        {
            get
            {
                if (m_eventSystem != null) return m_eventSystem;
                return UnityEngine.EventSystems.EventSystem.current;
            }
            set { m_eventSystem = value; }
        }

        /// <summary>
        /// Set false to prevent all UIButtonKeyTrigger components from listening for input.
        /// </summary>
        public static bool monitorInput = true;

        protected virtual void Awake()
        {
            m_selectable = GetComponent<UnityEngine.UI.Selectable>();
            if (m_selectable == null) enabled = false;
        }

#if USE_NEW_INPUT

        protected virtual void OnEnable()
        {
            if (inputAction != null)
            {
                inputAction.action.Enable();
                inputAction.action.performed += OnInputActionPerformed;
            }
        }

        protected virtual void OnDisable()
        {
            if (inputAction != null)
            {
                inputAction.action.performed -= OnInputActionPerformed;
                if (disableInputActionAfterClick) inputAction.action.Disable();
            }
        }

        private void OnInputActionPerformed(InputAction.CallbackContext context)
        {
            if (!monitorInput) return;
            if (!(m_selectable.enabled && m_selectable.interactable && m_selectable.gameObject.activeInHierarchy)) return;
            if (skipIfBeingClickedBySubmit && IsBeingClickedBySubmit()) return;
            Click();
        }

#else

        protected virtual void OnEnable() { }

        protected virtual void OnDisable() { }

#endif

        protected void Update()
        {
            if (!monitorInput) return;
            if (!(m_selectable.enabled && m_selectable.interactable && m_selectable.gameObject.activeInHierarchy)) return;
            if (InputDeviceManager.IsKeyDown(key) ||
                (!string.IsNullOrEmpty(buttonName) && InputDeviceManager.IsButtonDown(buttonName)) ||
                (anyKeyOrButton && InputDeviceManager.IsAnyKeyDown()))
            {
                if (skipIfBeingClickedBySubmit && IsBeingClickedBySubmit()) return;
                Click();
            }
        }

        protected virtual bool IsBeingClickedBySubmit()
        {
            return eventSystem != null &&
                eventSystem.currentSelectedGameObject == m_selectable.gameObject &&
                InputDeviceManager.instance != null &&
                InputDeviceManager.IsButtonDown(InputDeviceManager.instance.submitButton);
        }

        protected virtual void Click()
        {
            if (simulateButtonClick)
            {
                StartCoroutine(SimulateButtonClick());
            }
            else
            {
                ExecuteEvents.Execute(m_selectable.gameObject, new PointerEventData(eventSystem), ExecuteEvents.submitHandler);
            }
        }

        protected IEnumerator SimulateButtonClick()
        {
            m_selectable.OnPointerDown(new PointerEventData(eventSystem));
            var timeLeft = simulateButtonDownDuration;
            while (timeLeft > 0)
            {
                yield return null;
                timeLeft -= Time.unscaledDeltaTime;
            }
            m_selectable.OnPointerUp(new PointerEventData(eventSystem));
            m_selectable.OnDeselect(null);
            ExecuteEvents.Execute(m_selectable.gameObject, new PointerEventData(eventSystem), ExecuteEvents.submitHandler);
        }

    }

}
