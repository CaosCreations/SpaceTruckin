using UnityEngine;
#if USE_CINEMACHINE //2
using Cinemachine;
using CinemachineCam = Cinemachine.CinemachineVirtualCamera;
#elif USE_CINEMACHINE_3
using Unity.Cinemachine;
using CinemachineCam = Unity.Cinemachine.CinemachineCamera;
#endif

namespace PixelCrushers.DialogueSystem
{

#if USE_CINEMACHINE || USE_CINEMACHINE_3

    [AddComponentMenu("")] // Use wrapper.
    public class CinemachineCameraPriorityOnDialogueEvent : ActOnDialogueEvent
    {

        [Tooltip("The Cinemachine virtual camera whose priority to control.")]
        public CinemachineCam virtualCamera;

        [Tooltip("Set the virtual camera to this priority when the start event occurs.")]
        public int onStart = 99;

        [Tooltip("Set the virtual camera to this priority when the end event occurs.")]
        public int onEnd = 0;

        public override void TryStartActions(Transform actor)
        {
            if (virtualCamera == null) return;
            virtualCamera.Priority = onStart;
        }

        public override void TryEndActions(Transform actor)
        {
            if (virtualCamera == null) return;
            virtualCamera.Priority = onEnd;
        }
    }

#else

    [AddComponentMenu("")] // Use wrapper.
    public class CinemachineCameraPriorityOnDialogueEvent : ActOnDialogueEvent
    {
        public override void TryStartActions(Transform actor) { }
        public override void TryEndActions(Transform actor) { }
    }

#endif

}
