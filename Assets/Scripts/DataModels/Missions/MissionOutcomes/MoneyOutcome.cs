using UnityEngine;

[CreateAssetMenu(fileName = "MoneyOutcome", menuName = "ScriptableObjects/Missions/Outcomes/MoneyOutcome", order = 1)]
public class MoneyOutcome : MissionOutcome
{
    public float moneyMin;
    public float moneyMax;

    public override void Process(Mission mission)
    {
        PlayerManager.Instance.ReceiveMoney((long)Random.Range(moneyMin, moneyMax));
        Debug.Log("New balance: " + PlayerManager.Instance.Money);
    }
}