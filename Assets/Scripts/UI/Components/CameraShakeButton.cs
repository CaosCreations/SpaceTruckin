using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CameraShakeButton : MonoBehaviour, IPointerDownHandler
{
    [SerializeField]
    private Button button;

    [SerializeField]
    private CameraShakeSettings shakeSettings;

    private void Start()
    {
        if (shakeSettings == null)
            throw new System.Exception($"Mandatory {nameof(CameraShakeSettings)} property is not set.");

        //button.AddOnClick(ShakeCameraHandler, removeListeners: false);
    }

    private void ShakeCameraHandler()
    {
        StationCameraManager.Instance.ShakeLiveCamera(shakeSettings);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        ShakeCameraHandler();
    }
}