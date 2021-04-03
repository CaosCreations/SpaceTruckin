using UnityEngine;
using UnityEngine.UI;

public class RepairToolsUI : MonoBehaviour
{
    [SerializeField] private Text toolsText;
    [SerializeField] private Button buyButton;
    [SerializeField] private InputField quantityInput;

    [SerializeField] private Button stopStartButton;

    private void Start()
    {
        buyButton.AddOnClick(BuyTools);
    }

    private void OnEnable()
    {
        UpdateToolsText();
        quantityInput.text = "0";
    }

    public void UpdateToolsText()
    {
        toolsText.SetText("x" + PlayerManager.Instance.RepairTools.ToString());
    }

    private void BuyTools()
    {
        if (int.TryParse(quantityInput.text, out int numberOfToolsToBuy))
        {
            int costOfTools = numberOfToolsToBuy * RepairsConstants.CostPerTool;

            if (numberOfToolsToBuy > 0 && PlayerManager.Instance.CanSpendMoney(costOfTools))
            {
                PlayerManager.Instance.SpendMoney(costOfTools);
                PlayerManager.Instance.RepairTools += numberOfToolsToBuy;
                UpdateToolsText();
            }

            // Allow the player to start the minigame as they now have tools
            stopStartButton.interactable = PlayerManager.CanRepair;
        }
    }
}
