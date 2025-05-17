// Copyright (c) Pixel Crushers. All rights reserved.

using UnityEngine;

namespace PixelCrushers.DialogueSystem.SequencerCommands
{

    /// <summary>
    /// Implements sequencer command: Camera(angle[, gameobject|speaker|listener[, duration]])
    /// 
    /// Arguments:
    /// -# Name of a camera angle (child transform) defined in cameraAngles. If the angle isn't 
    /// defined in Sequencer.CameraAngles, looks for a game object in the scene. Default: Closeup.
    /// -# (Optional) The subject; can be speaker, listener, or the name of a game object. Default:
    /// speaker.
    /// -# (Optional) Duration over which to move the camera. Default: immediate.
    /// </summary>
    [AddComponentMenu("")] // Hide from menu.
    public class SequencerCommandCamera : SequencerCommand
    {

        protected const float SmoothMoveCutoff = 0.05f;

        protected Transform subject;
        protected Transform angleTransform;
        protected Transform cameraTransform;
        protected bool isLocalTransform;
        protected Quaternion targetRotation;
        protected Vector3 targetPosition;
        protected float duration;
        protected float startTime;
        protected float endTime;
        protected Quaternion originalRotation;
        protected Vector3 originalPosition;

        protected virtual void Start()
        {
            // Get the values of the parameters:
            string angle = GetParameter(0, "Closeup");
            subject = GetSubject(1);
            duration = GetParameterAsFloat(2, 0);

            // Get angle:
            bool isDefault = string.Equals(angle, "default");
            if (isDefault) angle = SequencerTools.GetDefaultCameraAngle(subject);
            bool isOriginal = string.Equals(angle, "original");
            angleTransform = isOriginal
                ? ((Camera.main != null) ? Camera.main.transform : speaker)
                : ((sequencer.cameraAngles != null) ? sequencer.cameraAngles.transform.Find(angle) : null);
            isLocalTransform = true;
            if (angleTransform == null)
            {
                isLocalTransform = false;
                angleTransform = SequencerTools.GetSubject(angle, speaker, listener, null);
            }

            // Log:
            if ((angleTransform == null) && DialogueDebug.logWarnings) Debug.LogWarning(string.Format("{0}: Sequencer: Camera({1}): Camera angle '{2}' wasn't found.", new System.Object[] { DialogueDebug.Prefix, GetParameters(), angle }));
            else if ((subject == null && !isOriginal) && !(isLocalTransform && angleTransform != null) && DialogueDebug.logWarnings) Debug.LogWarning(string.Format("{0}: Sequencer: Camera({1}): Camera subject '{2}' or GameObject named '{3}' wasn't found.", new System.Object[] { DialogueDebug.Prefix, GetParameters(), GetParameter(1), GetParameter(0) }));
            else if (DialogueDebug.logInfo) Debug.Log(string.Format("{0}: Sequencer: Camera({1}, {2}, {3}s)", new System.Object[] { DialogueDebug.Prefix, angle, Tools.GetGameObjectName(subject), duration }));

            // If we have a camera angle and subject, move the camera to it:
            sequencer.TakeCameraControl();
            if (isOriginal || (angleTransform != null && (subject != null || isLocalTransform)))
            {
                cameraTransform = sequencer.sequencerCameraTransform;
                if (isOriginal)
                {
                    targetRotation = sequencer.originalCameraRotation;
                    targetPosition = sequencer.originalCameraPosition;
                }
                else if (isLocalTransform)
                {
                    targetRotation = subject.rotation * angleTransform.localRotation;
                    targetPosition = subject.position + subject.rotation * angleTransform.localPosition;
                }
                else
                {
                    targetRotation = angleTransform.rotation;
                    targetPosition = angleTransform.position;
                }

                // If duration is above the cutoff, smoothly move camera toward camera angle:
                if (duration > SmoothMoveCutoff)
                {
                    startTime = DialogueTime.time;
                    endTime = startTime + duration;
                    originalRotation = cameraTransform.rotation;
                    originalPosition = cameraTransform.position;
                    var useUnscaledTime = DialogueTime.mode != DialogueTime.TimeMode.Gameplay;
                    var easing = DialogueManager.displaySettings.cameraSettings.cameraEasing;
                    Tweener.Tween(originalPosition, targetPosition, duration, useUnscaledTime, easing,
                        onBegin: null,
                        onValue: (x) => cameraTransform.position = x,
                        onEnd: Stop);
                    Tweener.Tween(originalRotation, targetRotation, duration, useUnscaledTime, easing,
                        onBegin: null,
                        onValue: (x) => cameraTransform.rotation = x,
                        onEnd: Stop);
                }
                else
                {
                    Stop();
                }
            }
            else
            {
                Stop();
            }
        }

        //--- We've switched to using the Tweener class to allow for more easing styles.
        //--- This is just a safeguard now:
        protected virtual void Update()
        {
            if (DialogueTime.time > endTime) Stop();
        }
        //public void Update()
        //{
        //    // Keep smoothing for the specified duration:
        //    if (DialogueTime.time < endTime)
        //    {
        //        float elapsed = (DialogueTime.time - startTime) / duration;
        //        cameraTransform.rotation = Quaternion.Lerp(originalRotation, targetRotation, elapsed);
        //        cameraTransform.position = Vector3.Lerp(originalPosition, targetPosition, elapsed);
        //    }
        //    else
        //    {
        //        Stop();
        //    }
        //}

        protected virtual void OnDestroy()
        {
            // Final position:
            if (angleTransform != null && subject != null)
            {
                cameraTransform.rotation = targetRotation;
                cameraTransform.position = targetPosition;
            }
        }

    }

}
