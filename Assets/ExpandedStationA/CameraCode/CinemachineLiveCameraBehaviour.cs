using Cinemachine;
using System.Linq;
using UnityEngine;

public abstract class CinemachineLiveCameraBehaviour : MonoBehaviour
{
    protected CinemachineBrain cinemachineBrain;
    protected CinemachineVirtualCamera virtualCamera;

    protected virtual void Awake()
    {
        cinemachineBrain = FindObjectOfType<CinemachineBrain>();
        UpdateActiveCamera();
    }

    protected virtual void UpdateActiveCamera()
    {
        if (cinemachineBrain.ActiveVirtualCamera == null)
        {
            return;
        }

        virtualCamera = GetLiveVirtualCamera();
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
            return stateDrivenCamera.ChildCameras.FirstOrDefault(vcam => CinemachineCore.Instance.IsLive(vcam)) as CinemachineVirtualCamera;
        }

        return cinemachineBrain.ActiveVirtualCamera.VirtualCameraGameObject.GetComponent<CinemachineVirtualCamera>();
    }
}