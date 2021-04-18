using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class BedCanvasUI : UICanvasBase
{
    [Header("Set at runtime")]
    public Image backgroundImage;

    private float timer;
    private const float timeToSleep = 4;
    private const float timeToDock = 2; 
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

        if(timer < timeToSleep / 2)
        {
            opacity += Time.deltaTime / (timeToSleep / 2);
        }
        else
        {
            opacity -= Time.deltaTime / (timeToSleep / 2);
        }

        backgroundImage.color = new Color(0, 0, 0, opacity);

        if (timer >= timeToSleep)
        {
            UIManager.ClearCanvases();
        }
    }

    private void EndDay()
    {
        StartCoroutine(WaitForShipsToDock());
        MissionsManager.UpdateMissionSchedule();
        OnEndOfDay?.Invoke();

        PlayerManager.Instance.SaveData();
        MissionsManager.Instance.SaveData();
        ArchivedMissionsManager.Instance.SaveData();
        PilotsManager.Instance.SaveData();
        ShipsManager.Instance.SaveData();
        MessagesManager.Instance.SaveData();
        LicencesManager.Instance.SaveData();
        HangarManager.Instance.SaveBatteryData();
    }

    private IEnumerator WaitForShipsToDock()
    {
        yield return new WaitForSeconds(timeToDock);
    }
}
