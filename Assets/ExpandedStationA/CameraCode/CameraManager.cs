using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance;

    private CinemachineVirtualCamera currentVirtualCamera;

    [SerializeField] private CinemachineVirtualCamera startingVirtualCamera;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        currentVirtualCamera = startingVirtualCamera;
    }

    public void SwitchCamera(CinemachineVirtualCamera newVirtualCamera)
    {
        currentVirtualCamera.Priority = 0;
        newVirtualCamera.Priority = 1;
        currentVirtualCamera = newVirtualCamera;
    }
}
