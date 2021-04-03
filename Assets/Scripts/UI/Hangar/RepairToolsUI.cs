using System;
using UnityEngine;
using UnityEngine.UI;

public class RepairToolsUI : MonoBehaviour
{
    [SerializeField] private Text toolsText;
    [SerializeField] private Button buyButton;
    [SerializeField] private InputField quantityInput;

    private void Start()
    {
        buyButton.AddOnClick(BuyTools);
    }

    public void UpdateToolsText()
    {
        toolsText.SetText("x" + PlayerManager.Instance.RepairTools.ToString());
    }

    private void BuyTools()
    {
        if (quantityInput != null )
        {
            int numberOfToolsToBuy = Convert.ToInt32(quantityInput.text);
            long costOfTools = numberOfToolsToBuy * RepairsConstants.CostPerTool;
            
            if (numberOfToolsToBuy > 0 && PlayerManager.Instance.CanSpendMoney(costOfTools))
            {
                PlayerManager.Instance.SpendMoney(costOfTools);
                PlayerManager.Instance.RepairTools += numberOfToolsToBuy;
                UpdateToolsText();
            }
        }
    }
}