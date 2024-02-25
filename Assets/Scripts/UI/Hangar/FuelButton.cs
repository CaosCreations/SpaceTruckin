using Events;
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

    public void Init(Ship ship, long costPerUnit)
    {
        if (Button == null)
        {
            Button = GetComponent<Button>();
        }
        shipToFuel = ship;
        fuelCostPerUnit = costPerUnit;
    }

    private void StartFueling()
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
        SingletonManager.EventService.Dispatch<OnFuelingStartedEvent>();
    }

    public void StopFueling()
    {
        IsFueling = false;

        if (screenShake != null)
        {
            screenShake.StopShake();
        }
        Debug.Log($"OnFuelingEnded for {shipToFuel} at {shipToFuel.CurrentFuel} fuel");
        SingletonManager.EventService.Dispatch(new OnFuelingEndedEvent(shipToFuel.CurrentFuel));
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        StartFueling();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        StopFueling();
    }
}
