using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class BedCanvasUI : UICanvasBase
{
    [Header("Set at runtime")]
    [SerializeField] private Image backgroundImage;

    private float timer;
    private float opacity;

    public static UnityAction OnEndOfDay;

    private void Awake()
    {
        backgroundImage = GetComponent<Image>();
    }

    private void OnEnable()
    {
        timer = 0;
        opacity = 0;
        EndDay();
    }

    void Update()
    {
        timer += Time.deltaTime;

        if(timer < UIConstants.TimeToSleep / 2)
        {
            opacity += Time.deltaTime / (UIConstants.TimeToSleep / 2);
        }
        else
        {
            opacity -= Time.deltaTime / (UIConstants.TimeToSleep / 2);
        }

        backgroundImage.color = new Color(0, 0, 0, opacity);

        if (timer >= UIConstants.TimeToSleep)
        {
            UIManager.ClearCanvases();
        }
    }

    private void EndDay()
    {
        StartCoroutine(WaitForShipsToDock());
        MissionsManager.UpdateMissionSchedule();
        OnEndOfDay?.Invoke();
        SaveAllData();
    }

    private void SaveAllData()
    {
        PlayerManager.Instance.SaveData();
        MissionsManager.Instance.SaveData();
        ArchivedMissionsManager.Instance.SaveData();
        PilotsManager.Instance.SaveData();
        ShipsManager.Instance.SaveData();
        HangarManager.Instance.SaveBatteryData();
        MessagesManager.Instance.SaveData();
        LicencesManager.Instance.SaveData();
    }

    private IEnumerator WaitForShipsToDock()
    {
        yield return new WaitForSeconds(UIConstants.TimeToDock);
    }
}
