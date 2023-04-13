using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FuelButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public Button Button { get; private set; }
    public bool IsFueling { get; private set; }

    [SerializeField]
    private CameraShakeSettings shakeSettings;

    [SerializeField]
    private CameraScreenShake screenShake;

    private Ship shipToFuel;
    private long fuelCostPerUnit;

    private void Awake()
    {
        Button = GetComponent<Button>();
    }

    public void Init(Ship ship, long costPerUnit)
    {
        shipToFuel = ship;
        fuelCostPerUnit = costPerUnit;
    }

    private void StartFuelling()
    {
        if (shipToFuel == null || !UIUtils.IsFuelButtonInteractable(shipToFuel, fuelCostPerUnit))
        {
            IsFueling = false;
            return;
        }

        IsFueling = true;

        if (screenShake != null && shakeSettings != null)
        {
            screenShake.Shake(shakeSettings);
        }
    }

    public void StopFuelling()
    {
        IsFueling = false;

        if (screenShake != null)
        {
            screenShake.StopShake();
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        StartFuelling();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        StopFuelling();
    }
}
