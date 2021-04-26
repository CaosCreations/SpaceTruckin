using UnityEngine;
using UnityEngine.UI;

public class RepairToolsUI : MonoBehaviour
{
    [SerializeField] private Text toolsText;
    [SerializeField] private Text toolsCostText;
    [SerializeField] private Button buyButton;
    [SerializeField] private InputField quantityInput;

    private int currentQuantity;
    private int TotalCost => currentQuantity * RepairsConstants.CostPerTool;

    [SerializeField] private Button stopStartRepairsButton;

    private void Start()
    {
        buyButton.AddOnClick(BuyTools);
        quantityInput.AddOnValueChanged(HandleOnValueChanged);
    }

    private void OnEnable()
    {
        ResetValues();
    }

    private void BuyTools()
    {
        if (currentQuantity > 0 && PlayerManager.Instance.CanSpendMoney(TotalCost))
        {
            PlayerManager.Instance.SpendMoney(TotalCost);
            PlayerManager.Instance.RepairTools += currentQuantity;
            stopStartRepairsButton.interactable = PlayerManager.CanRepair;
            UpdateToolsText();
            UpdateToolsCostText();
        }
        else
        {
            LogErrors();
        }
    }

    private void HandleOnValueChanged()
    {
        currentQuantity = GetCurrentQuantity();
        UpdateToolsCostText();
    }

    public void UpdateToolsText()
    {
        toolsText.SetText("x" + PlayerManager.Instance.RepairTools.ToString());
    }

    private int GetCurrentQuantity()
    {
        if (!string.IsNullOrWhiteSpace(quantityInput.text)
            && int.TryParse(quantityInput.text, out int quantity))
        {
            return quantity;
        }

        // Empty string/whitespace equates to 0
        return 0;
    }

    private void UpdateToolsCostText()
    {
        toolsCostText.SetText(RepairsConstants.ToolsCostText + TotalCost.ToString());
        UpdateToolsCostColour(PlayerManager.Instance.CanSpendMoney(TotalCost));
    }

    private void UpdateToolsCostColour(bool canAfford)
    {
        Color toolsCostColour;

        if (currentQuantity <= 0)
        {
            toolsCostColour = UIConstants.SpringWood;
        }
        else if (canAfford)
        {
            toolsCostColour = UIConstants.ChelseaCucumber;
        }
        else
        {
            toolsCostColour = UIConstants.Matrix;
        }

        toolsCostText.color = toolsCostColour;
    }

    private void ResetValues()
    {
        UpdateToolsText();
        currentQuantity = 0;
        quantityInput.text = "0";
        toolsCostText.SetText(RepairsConstants.ToolsCostText + 0.ToString());
        toolsCostText.color = UIConstants.SpringWood;
    }

    #region Diagnostics
    private void LogErrors()
    {
        if (currentQuantity <= 0)
        {
            Debug.LogError("Tool quantity was 0. Cannot purchase 0 tools.");
        }

        if (!int.TryParse(quantityInput.text, out _))
        {
            Debug.LogError($"Invalid tool input type (must be int). Value was: '{quantityInput.text}'");
        }
    }
    #endregion
}
