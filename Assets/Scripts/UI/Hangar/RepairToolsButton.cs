using Events;
using UnityEngine;
using UnityEngine.UI;

public class RepairToolsButton : MonoBehaviour
{
    private Button button;
    private Text buttonText;

    private void Start()
    {
        button = GetComponent<Button>();
        button.AddOnClick(BuyTool);
        buttonText = button.GetComponentInChildren<Text>();
        SetText();

        SingletonManager.EventService.Add<OnRepairsToolSpentEvent>(SetText);
    }

    private void SetText()
    {
        buttonText.SetText("Tools: " + PlayerManager.Instance.RepairTools, FontType.Button);
    }

    private void BuyTool()
    {
        if (PlayerManager.Instance.CanSpendMoney(RepairsConstants.CostPerTool))
        {
            PlayerManager.Instance.BuyRepairTools(1);
            SetText();
        }
        else
        {
            Debug.LogWarning("Not enough money to buy repair tools.");
        }
    }
}
