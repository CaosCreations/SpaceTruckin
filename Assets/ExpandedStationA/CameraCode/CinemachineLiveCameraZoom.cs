using Cinemachine;
using System;
using System.Collections;
using UnityEngine;

public class CinemachineLiveCameraZoom : CinemachineLiveCameraBehaviour
{
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

    protected override void UpdateActiveCamera()
    {
        base.UpdateActiveCamera();
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