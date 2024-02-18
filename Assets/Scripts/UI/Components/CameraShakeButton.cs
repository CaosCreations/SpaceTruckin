using UnityEngine;
using UnityEngine.UI;

public class CameraShakeButton : MonoBehaviour
{
    [SerializeField]
    private Button button;

    [SerializeField]
    private CameraShakeSettings shakeSettings;

    private void Start()
    {
        if (shakeSettings == null)
            throw new System.Exception($"Mandatory {nameof(CameraShakeSettings)} property is not set.");

        button.AddOnClick(ShakeCameraHandler, removeListeners: false);
    }

    private void ShakeCameraHandler()
    {
        StationCameraManager.ShakeLiveCamera(shakeSettings);
    }

    //public void OnPointerDown(PointerEventData eventData)
    //{
    //    ShakeCameraHandler();
    //}
}