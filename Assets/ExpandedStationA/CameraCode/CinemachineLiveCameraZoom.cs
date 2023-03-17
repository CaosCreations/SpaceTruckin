using Cinemachine;
using Events;
using System.Linq;
using UnityEngine;

public class CinemachineLiveCameraZoom : MonoBehaviour
{
    private CinemachineBrain cinemachineBrain;
    private CinemachineVirtualCamera virtualCamera;
    private CinemachineFramingTransposer framingTransposer;
    private float speed;
    private float startingDistance;
    private float targetDistance;

    [SerializeField]
    private float staticTargetDistance;

    [SerializeField]
    private float staticSpeed;

    [SerializeField]
    private bool start = false;

    private void Awake()
    {
        cinemachineBrain = FindObjectOfType<CinemachineBrain>();
        UpdateActiveCamera();
    }

    private void UpdateActiveCamera()
    {
        if (cinemachineBrain.ActiveVirtualCamera == null)
        {
            return;
        }

        virtualCamera = GetLiveVirtualCamera();
        framingTransposer = virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
        startingDistance = framingTransposer.m_CameraDistance;
    }

    public void ZoomInCamera(float targetDistance, float speed)
    {
        UpdateActiveCamera();
        this.targetDistance = targetDistance;
        this.speed = speed;
    }

    public void ResetZoom()
    {
        framingTransposer.m_CameraDistance = startingDistance;
    }

    public CinemachineVirtualCamera GetLiveVirtualCamera()
    {
        if (cinemachineBrain.ActiveVirtualCamera == null)
        {
            return default;
        }

        // Select child camera if the active camera is a state driven camera 
        if (cinemachineBrain.ActiveVirtualCamera.VirtualCameraGameObject.TryGetComponent<CinemachineStateDrivenCamera>(out var stateDrivenCamera))
        {
            return stateDrivenCamera.ChildCameras.FirstOrDefault(cam => cam.Follow == PlayerManager.PlayerObject.transform) as CinemachineVirtualCamera;
        }

        return cinemachineBrain.ActiveVirtualCamera.VirtualCameraGameObject.GetComponent<CinemachineVirtualCamera>();
    }

    private void Update()
    {
        if (start)
        {
            ZoomInCamera(3f, 1.2f);
            start = false;
        }

        if (framingTransposer == null || framingTransposer.m_CameraDistance <= targetDistance)
        {
            return;
        }

        framingTransposer.m_CameraDistance -= Time.deltaTime * speed;

        if (framingTransposer.m_CameraDistance <= targetDistance)
        {
            framingTransposer.m_CameraDistance = targetDistance;
            SingletonManager.EventService.Dispatch<OnLiveCameraZoomInEndedEvent>();
        }
    }
}