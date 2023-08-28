using System;
using System.Linq;
using UnityEngine;

public class StationCameraManager : MonoBehaviour
{
    public static StationCameraManager Instance { get; private set; }

    private StationCamera[] stationCameras;
    private CinemachineLiveCameraZoom liveCameraZoom;
    private CinemachineLiveCameraShake liveCameraShake;
    [SerializeField] private CamAnimStateChange animStateChange;

    public static bool IsLiveCameraZooming;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        stationCameras = FindObjectsOfType<StationCamera>(true);
        liveCameraZoom = GetComponent<CinemachineLiveCameraZoom>();
        liveCameraShake = GetComponent<CinemachineLiveCameraShake>();
    }

    public void ShakeCamera(StationCamera.Identifier cameraIdentifier)
    {
        var camera = GetCameraByIdentifier(cameraIdentifier);
        if (camera == null)
        {
            Debug.LogError("Camera with identifier " + cameraIdentifier + " does not exist.");
            return;
        }

        camera.ShakeCamera();
    }

    public void ZoomInLiveCamera(float targetDistance, float speed)
    {
        liveCameraZoom.ZoomInCamera(targetDistance, speed);
    }

    public void ZoomInLiveCamera(float targetDistance, float speed, Action action)
    {
        liveCameraZoom.ZoomInCamera(targetDistance, speed, action);
    }

    public void ZoomInLiveCamera(CameraZoomSettings settings)
    {
        liveCameraZoom.ZoomInCamera(settings.TargetDistance, settings.Speed);
    }

    public void ZoomInLiveCamera(CameraZoomSettings settings, Action action)
    {
        liveCameraZoom.ZoomInCamera(settings.TargetDistance, settings.Speed, action, settings.ResetAfter, settings.HidePlayer, settings.LockPlayer);
    }

    public void ResetLiveCameraZoom()
    {
        liveCameraZoom.ResetZoom();
    }

    public void ShakeLiveCamera(CameraShakeSettings settings)
    {
        liveCameraShake.Shake(settings);
    }

    public void ShakeLiveCamera(float amplitude)
    {
        liveCameraShake.Shake(amplitude);
    }

    public void PlayCamAnimState(string stateName)
    {
        animStateChange.PlayState(stateName);
    }

    private StationCamera GetCameraByIdentifier(StationCamera.Identifier cameraIdentifier)
    {
        return stationCameras.FirstOrDefault(cam => cam.CameraIdentifier == cameraIdentifier);
    }
}
