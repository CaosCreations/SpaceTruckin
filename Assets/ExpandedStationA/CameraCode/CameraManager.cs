using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour
{
    private CinemachineVirtualCamera currentVirtualCamera;

    [SerializeField] private CinemachineVirtualCamera startingVirtualCamera;

    private void Awake()
    {
        currentVirtualCamera = startingVirtualCamera;
    }

    public void SwitchCamera(CinemachineVirtualCamera newVirtualCamera)
    {
        currentVirtualCamera.Priority = 0;
        newVirtualCamera.Priority = 1;
        currentVirtualCamera = newVirtualCamera;
    }
}
