using UnityEngine;
using UnityEngine.UI;

public class NewDayReportDetailsCard : MonoBehaviour
{
    [SerializeField] protected Text detailsText;
    [SerializeField] protected Animator animator;

    public void SetText(string text)
    {
        // Text is optional 
        if (detailsText == null)
            return;
        detailsText.SetText(text);
    }

    public void ShowDetails()
    {
        gameObject.SetActive(true);
        // TODO: Play animation when it comes in 

    }
}
