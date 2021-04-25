using UnityEngine;
using UnityEngine.UI;

public class RepairToolsUI : MonoBehaviour
{
    [SerializeField] private Text toolsText;
    [SerializeField] private Text toolsCostText;
    [SerializeField] private Button buyButton;
    [SerializeField] private InputField quantityInput;

    [SerializeField] private Button stopStartButton;

    private void Start()
    {
        buyButton.AddOnClick(BuyTools);
        quantityInput.onValueChanged.AddListener(delegate { UpdateToolsCostText(); });
    }

    private void OnEnable()
    {
        UpdateToolsText();
        quantityInput.text = "0";
        toolsCostText.SetText(RepairsConstants.ToolsCostText + 0.ToString());
    }

    private void BuyTools()
    {
        if (int.TryParse(quantityInput.text, out int quantity))
        {
            int costOfTools = GetTotalCost(quantity);

            if (quantity > 0 && PlayerManager.Instance.CanSpendMoney(costOfTools))
            {
                PlayerManager.Instance.SpendMoney(costOfTools);
                PlayerManager.Instance.RepairTools += quantity;
                stopStartButton.interactable = PlayerManager.CanRepair;
                UpdateToolsText();
                UpdateToolsCostText();
            }
        }
        else
        {
            Debug.LogError($"Invalid input when buying tools (must be int). Value was: '{quantityInput.text}'");
        }
    }

    private int GetTotalCost(int quantity)
    {
        return quantity * RepairsConstants.CostPerTool;
    }

    public void UpdateToolsText()
    {
        toolsText.SetText("x" + PlayerManager.Instance.RepairTools.ToString());
    }

    private void UpdateToolsCostText()
    {
        int newCost = 0;
        
        // Empty string/whitespace equates to 0
        if (!string.IsNullOrWhiteSpace(quantityInput.text)
            && int.TryParse(quantityInput.text, out int quantity))
        {
            newCost = GetTotalCost(quantity);
        }

        toolsCostText.SetText(RepairsConstants.ToolsCostText + newCost.ToString());
        UpdateToolsCostColour(PlayerManager.Instance.CanSpendMoney(newCost));
    }

    private void UpdateToolsCostColour(bool canAfford)
    {
        toolsCostText.color = canAfford ? UIConstants.ChelseaCucumber : UIConstants.Matrix;
    }
}
