using Cinemachine;
using System;
using System.Linq;
using UnityEngine;

public class StationCameraManager : MonoBehaviour
{
    private static StationCamera[] stationCameras;
    private static CinemachineBrain cinemachineBrain;
    private static CinemachineLiveCameraZoom liveCameraZoom;
    private static CinemachineLiveCameraShake liveCameraShake;
    private static CamAnimStateChange animStateChange;

    public static bool IsLiveCameraZooming;

    private void Awake()
    {
        stationCameras = FindObjectsOfType<StationCamera>(true);
        cinemachineBrain = FindObjectOfType<CinemachineBrain>(true);
        liveCameraZoom = GetComponent<CinemachineLiveCameraZoom>();
        liveCameraShake = GetComponent<CinemachineLiveCameraShake>();
        animStateChange = GetComponent<CamAnimStateChange>();
    }

    public static void ShakeCamera(StationCamera.Identifier cameraIdentifier)
    {
        var camera = GetCameraByIdentifier(cameraIdentifier);
        if (camera == null)
        {
            Debug.LogError("Camera with identifier " + cameraIdentifier + " does not exist.");
            return;
        }

        camera.ShakeCamera();
    }

    public static void ZoomInLiveCamera(float targetDistance, float speed)
    {
        liveCameraZoom.ZoomInCamera(targetDistance, speed);
    }

    public static void ZoomInLiveCamera(float targetDistance, float speed, Action action)
    {
        liveCameraZoom.ZoomInCamera(targetDistance, speed, action);
    }

    public static void ZoomInLiveCamera(CameraZoomSettings settings)
    {
        liveCameraZoom.ZoomInCamera(settings.TargetDistance, settings.Speed);
    }

    public static void ZoomInLiveCamera(CameraZoomSettings settings, Action action)
    {
        liveCameraZoom.ZoomInCamera(settings.TargetDistance, settings.Speed, action, settings.ResetAfter, settings.HidePlayer, settings.LockPlayer);
    }

    public static void ResetLiveCameraZoom()
    {
        liveCameraZoom.ResetZoom();
    }

    public static void ShakeLiveCamera(CameraShakeSettings settings)
    {
        liveCameraShake.Shake(settings);
    }

    public static void ShakeLiveCamera(float amplitude)
    {
        liveCameraShake.Shake(amplitude);
    }

    public static void PlayCamAnimState(string stateName)
    {
        animStateChange.PlayState(stateName);
    }

    public static void SetBlend(CinemachineBlendDefinition.Style style, float time)
    {
        cinemachineBrain.m_DefaultBlend = new CinemachineBlendDefinition(style, time);
    }

    private static StationCamera GetCameraByIdentifier(StationCamera.Identifier cameraIdentifier)
    {
        return stationCameras.FirstOrDefault(cam => cam.CameraIdentifier == cameraIdentifier);
    }
}
