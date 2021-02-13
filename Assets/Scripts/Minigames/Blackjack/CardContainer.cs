using UnityEngine;
using UnityEngine.UI;

public class CardContainer : MonoBehaviour
{
    public Text totalText;
    public Text chipsText;
    public Text wagerText;

    public string TotalText { get => totalText.text; set => totalText.text = value; }

    public void DestroyCards()
    {
        foreach (Transform child in transform)
        {
            if (child.gameObject.GetComponent<Image>())
            {
                Destroy(child.gameObject);
            }
        }
    }
}
