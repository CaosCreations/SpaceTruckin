using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BedCanvasUI : MonoBehaviour
{
    [Header("Set at runtime")]
    public Image backgroundImage;

    private float timer;
    private const float timeToSleep = 4;
    private float opacity;

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
        MissionsManager.UpdateMissionSchedule();
        ShipsManager.UpdateHangarShips();
    }
}
