using Cinemachine;
using UnityEngine;
using UnityEngine.Events;

public class CinemachineCameraZoom : MonoBehaviour
{
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

    public UnityAction OnZoomInEnded;

    private void Awake()
    {
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
        framingTransposer = virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
        startingDistance = framingTransposer.m_CameraDistance;
    }

    public void ZoomInCamera()
    {
        ZoomInCamera(staticTargetDistance, staticSpeed);
    }

    public void ZoomInCamera(float targetDistance, float speed)
    {
        startingDistance = framingTransposer.m_CameraDistance;
        this.targetDistance = targetDistance;
        this.speed = speed;
    }

    public void ResetZoom()
    {
        framingTransposer.m_CameraDistance = startingDistance;
    }

    private void Update()
    {
        if (start)
        {
            ZoomInCamera();
            start = false;
        }

        if (framingTransposer.m_CameraDistance <= targetDistance)
        {
            return;
        }

        framingTransposer.m_CameraDistance -= Time.deltaTime * speed;

        if (framingTransposer.m_CameraDistance <= targetDistance)
        {
            framingTransposer.m_CameraDistance = targetDistance;
            OnZoomInEnded?.Invoke();
        }
    }
}