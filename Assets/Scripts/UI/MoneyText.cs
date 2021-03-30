using UnityEngine;
using UnityEngine.UI;

public class MoneyText : MonoBehaviour
{
    [SerializeField] private Text moneyAsText;

    private void Start()
    {
        UpdateMoneyText();
        PlayerManager.OnFinancialTransaction += UpdateMoneyText;
    }

    private void UpdateMoneyText()
    {
        moneyAsText.SetText("$ " + PlayerManager.Instance.Money, FontType.Title);
    }
}
