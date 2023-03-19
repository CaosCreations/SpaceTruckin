using Events;
using UnityEngine;

public class StationCamera : MonoBehaviour
{
    public enum Identifier
    {
        Hangar = 0
    }

    public Identifier CameraIdentifier;
    private CinemachineCameraShake cameraShake;
    private CinemachineCameraZoom cameraZoom;

    private void Start()
    {
        cameraShake = GetComponent<CinemachineCameraShake>();
        cameraZoom = GetComponent<CinemachineCameraZoom>();
    }

    public void ShakeCamera()
    {
        if (cameraShake == null)
        {
            Debug.LogError(nameof(CinemachineCameraShake) + " component is null. Unable to shake camera with identifier: " + CameraIdentifier);
            return;
        }

        cameraShake.Shake();
    }
    
    public void ZoomInCamera()
    {
        if (cameraZoom == null)
        {
            Debug.LogError(nameof(CinemachineCameraZoom) + " component is null. Unable to shake camera with identifier: " + CameraIdentifier);
            return;
        }

        cameraZoom.ZoomInCamera();
    }

    private void OnZoomInEndedHandler()
    {
        SingletonManager.EventService.Dispatch(new OnCameraZoomInEndedEvent(CameraIdentifier));
    }
}