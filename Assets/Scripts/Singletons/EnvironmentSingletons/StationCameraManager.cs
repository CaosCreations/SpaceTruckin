using System;
using System.Linq;
using UnityEngine;

public class StationCameraManager : MonoBehaviour
{
    public static StationCameraManager Instance { get; private set; }

    private StationCamera[] stationCameras;
    private CinemachineLiveCameraZoom liveCameraZoom;

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
        liveCameraZoom.ZoomInCamera(settings.TargetDistance, settings.Speed, action);
    }

    public void ResetLiveCameraZoom()
    {
        liveCameraZoom.ResetZoom();
    }

    private StationCamera GetCameraByIdentifier(StationCamera.Identifier cameraIdentifier)
    {
        return stationCameras.FirstOrDefault(cam => cam.CameraIdentifier == cameraIdentifier);
    }
}