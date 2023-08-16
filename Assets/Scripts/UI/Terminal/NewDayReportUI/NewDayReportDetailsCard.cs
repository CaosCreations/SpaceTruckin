using UnityEngine;
using UnityEngine.UI;

public class NewDayReportDetailsCard : MonoBehaviour
{
    [SerializeField] protected Text detailsText;
    [SerializeField] protected Animator animator;
    [SerializeField] private Image image;
    private string text;

    public void SetText(string txt)
    {
        text = txt;
    }

    public void ShowDetails()
    {
        image.color = new Color(image.color.r, image.color.g, image.color.b, 255);
        // Text is optional 
        if (detailsText != null)
        {
            detailsText.gameObject.SetActive(true);
            detailsText.SetText(text);
        }
        // TODO: Play animation when it comes in 
    }

    public void HideDetails()
    {
        image.color = new Color(image.color.r, image.color.g, image.color.b, 0);
        if (detailsText != null)
        {
            detailsText.gameObject.SetActive(false);
            detailsText.Clear();
        }
    }
}
