using UnityEngine;

public class StationCamera : MonoBehaviour
{
    public enum Identifier
    {
        Hangar = 0
    }

    public Identifier CameraIdentifier;
    private CinemachineCameraShake cameraShake;

    private void Start()
    {
        cameraShake = GetComponent<CinemachineCameraShake>();
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
}