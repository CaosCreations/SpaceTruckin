using System.Collections.Generic;
using UnityEngine;
#if USE_NEW_INPUT
using UnityEngine.InputSystem;
#endif

namespace PixelCrushers
{

    /// <summary>
    /// Registers Input System actions with the Pixel Crushers
    /// Input Device Manager.
    /// </summary>
    [AddComponentMenu("")] // Use wrapper.
    public class InputActionRegistry : MonoBehaviour
    {

#if USE_NEW_INPUT

        [SerializeField] private List<InputActionReference> inputActions;

        public List<InputActionReference> InputActions => inputActions;

        private void Start()
        {
            RegisterInputActions();
        }

        private void RegisterInputActions()
        {
            foreach (var inputAction in InputActions)
            {
                if (inputAction == null) continue;
                InputDeviceManager.RegisterInputAction(inputAction.action.name, inputAction.action);
            }
        }

#endif

    }
}
