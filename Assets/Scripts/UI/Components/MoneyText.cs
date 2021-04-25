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

    private void OnEnable() => UpdateMoneyText();

    public void UpdateMoneyText()
    {
        moneyAsText.SetText("$ " + PlayerManager.Instance.Money, FontType.Title);
    }
}
