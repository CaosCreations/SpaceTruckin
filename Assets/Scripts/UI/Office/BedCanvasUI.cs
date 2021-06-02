using UnityEngine;
using UnityEngine.UI;

public class BedCanvasUI : UICanvasBase
{
    private Image backgroundImage;

    private float timer;
    private float opacity;

    private void Awake()
    {
        backgroundImage = GetComponent<Image>();
    }

    private void OnEnable()
    {
        timer = 0;
        opacity = 0;
        CalendarManager.EndDay();
    }

    private void Update()
    {
        timer += Time.deltaTime;

        if (timer < UIConstants.TimeToSleep / 2)
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
}
