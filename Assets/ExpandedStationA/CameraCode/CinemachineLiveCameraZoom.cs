﻿using Cinemachine;
using System;
using System.Collections;
using System.Linq;
using UnityEngine;

public class CinemachineLiveCameraZoom : MonoBehaviour
{
    private CinemachineBrain cinemachineBrain;
    private CinemachineVirtualCamera virtualCamera;
    private CinemachineFramingTransposer framingTransposer;
    private float startingDistance;

    [SerializeField]
    private float targetDistance;

    [SerializeField]
    private float speed;

    [SerializeField]
    private bool start = false;

    [SerializeField]
    private bool reset = false;

    private float CurrentDistance
    {
        get => framingTransposer.m_CameraDistance; set => framingTransposer.m_CameraDistance = value;
    }

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
        startingDistance = CurrentDistance;
    }

    public void ZoomInCamera(float targetDistance, float speed, Action action = null, bool resetAfter = false, bool hidePlayer = false, bool lockPlayer = false)
    {
        UpdateActiveCamera();
        StartCoroutine(ZoomInCameraRoutine(targetDistance, speed, action, resetAfter, hidePlayer, lockPlayer));
    }

    public void ResetZoom()
    {
        CurrentDistance = startingDistance;
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

    private IEnumerator ZoomInCameraRoutine(float targetDistance, float speed, Action action = null, bool resetAfter = false, bool hidePlayer = false, bool lockPlayer = false)
    {
        UpdateActiveCamera();

        if (hidePlayer)
        {
            PlayerManager.SetSpriteRendererEnabled(false);
        }

        if (lockPlayer)
        {
            PlayerManager.EnterPausedState();
        }

        while (CurrentDistance >= targetDistance)
        {
            CurrentDistance -= Time.deltaTime * speed;
            yield return null;
        }
        CurrentDistance = targetDistance;

        if (lockPlayer)
        {
            PlayerManager.ExitPausedState();
        }

        action?.Invoke();

        if (hidePlayer)
        {
            PlayerManager.SetSpriteRendererEnabled(true);
        }

        if (resetAfter)
        {
            ResetZoom();
        }
    }

    private void Update()
    {
        if (start)
        {
            ZoomInCamera(targetDistance, speed);
            start = false;
        }

        if (reset && virtualCamera != null)
        {
            ResetZoom();
            reset = false;
        }
    }

    private void OnValidate()
    {
        speed = Mathf.Max(1f, speed);
    }
}