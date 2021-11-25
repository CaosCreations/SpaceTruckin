using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraSwitch : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera virtualCamera;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag(PlayerConstants.PlayerTag) == true)
        {
            CameraManager.Instance.SwitchCamera(virtualCamera);
        }
    }
}
