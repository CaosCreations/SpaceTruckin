// Copyright (c) Pixel Crushers. All rights reserved.

using UnityEngine;

namespace PixelCrushers.DialogueSystem
{

    /// <summary>
    /// Allows toggling of the quest log window using a key or button,
    /// or by calling ToggleQuestLogWindow.
    /// </summary>
    [AddComponentMenu("")] // Use wrapper.
    public class QuestLogWindowHotkey : MonoBehaviour
    {

        [Tooltip("Toggle the quest log window when this key is pressed.")]
        public KeyCode key = KeyCode.J;

        [Tooltip("Toggle the quest log window when this input button is presed.")]
        public string buttonName = string.Empty;

        [Tooltip("(Optional) Use this quest log window. If unassigned, will automatically find quest log window in scene. If you assign a window, assign a scene instance, not an uninstantiated prefab.")]
        public QuestLogWindow questLogWindow;

        public QuestLogWindow runtimeQuestLogWindow
        {
            get
            {
                if (questLogWindow == null) questLogWindow = PixelCrushers.GameObjectUtility.FindFirstObjectByType<QuestLogWindow>();
                return questLogWindow;
            }
        }

        private void Awake()
        {
            if (questLogWindow == null) questLogWindow = PixelCrushers.GameObjectUtility.FindFirstObjectByType<QuestLogWindow>();
        }

#if USE_NEW_INPUT

        public UnityEngine.InputSystem.InputActionReference inputAction;

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
            }
        }

        private void OnInputActionPerformed(UnityEngine.InputSystem.InputAction.CallbackContext context)
        {
            if (runtimeQuestLogWindow == null) return;
            if (DialogueManager.IsDialogueSystemInputDisabled()) return;
            ToggleQuestLogWindow();
        }

#endif

        private void Update()
        {
            if (runtimeQuestLogWindow == null) return;
            if (DialogueManager.IsDialogueSystemInputDisabled()) return;
            if (InputDeviceManager.IsKeyDown(key) ||
                (!string.IsNullOrEmpty(buttonName) && DialogueManager.getInputButtonDown(buttonName)))
            {
                ToggleQuestLogWindow();
            }
        }

        public void ToggleQuestLogWindow()
        {
            if (runtimeQuestLogWindow == null) return;
            if (runtimeQuestLogWindow.isOpen)
            {
                runtimeQuestLogWindow.Close();
            }
            else
            {
                runtimeQuestLogWindow.Open();
            }
        }

    }

}
