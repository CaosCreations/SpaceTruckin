using UnityEngine;
using UnityEngine.UI;

public class MoneyText : MonoBehaviour
{
    [Header("Current funds")]
    [SerializeField] private Text moneyAsText;

    [Header("Total earnings")]
    [SerializeField] private Text totalEarningsAsText;
    [SerializeField] private bool showTotalEarnings;

    [Header("Text to show before amount")]
    [SerializeField] private string prefix;
    [SerializeField] private string totalEarningsPrefix;

    private readonly string defaultPrefix = "$ ";

    private void Start()
    {
        if (string.IsNullOrWhiteSpace(prefix))
            prefix = defaultPrefix;

        if (string.IsNullOrWhiteSpace(totalEarningsPrefix))
            totalEarningsPrefix = defaultPrefix;

        UpdateMoneyText();
        PlayerManager.OnMoneySpent += UpdateMoneyText;
    }

    private void OnEnable() => UpdateMoneyText();

    public void UpdateMoneyText()
    {
        moneyAsText.SetText(prefix + PlayerManager.Instance.Money, FontType.Title);

        if (showTotalEarnings)
        {
            totalEarningsAsText.SetText(totalEarningsPrefix + PlayerManager.Instance.TotalMoneyAcquired, FontType.Title);
        }
    }
}
