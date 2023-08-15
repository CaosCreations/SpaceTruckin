using UnityEngine;
using UnityEngine.UI;

public class NewDayReportDetailsCard : MonoBehaviour
{
    [SerializeField] protected Text detailsText;
    [SerializeField] protected Animator animator;

    public void SetText(string text)
    {
        detailsText.SetText(text);
    }

    public void ShowDetails()
    {
        gameObject.SetActive(true);
        // TODO: Play animation when it comes in 

    }
}
