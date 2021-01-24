using UnityEngine;

[CreateAssetMenu(fileName = "MoneyOutcome", menuName = "ScriptableObjects/Missions/Outcomes/MoneyOutcome", order = 1)]
public class MoneyOutcome : MissionOutcome
{
    [SerializeField] private long moneyMin;
    [SerializeField] private long moneyMax;

    public new MoneyOutcomeSaveData saveData;

    [System.Serializable]
    public class MoneyOutcomeSaveData : MissionOutcomeSaveData
    {
        public long moneyReceived;
    }

    public long MoneyReceived
    {
        get => saveData.moneyReceived; set => saveData.moneyReceived = value;
    }

    public override void Process(Mission mission)
    {
        MoneyReceived = (long)Random.Range(moneyMin, moneyMax);
        PlayerManager.Instance.ReceiveMoney(MoneyReceived);
    }
}