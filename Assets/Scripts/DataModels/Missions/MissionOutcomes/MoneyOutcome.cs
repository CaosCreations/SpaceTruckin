using UnityEngine;

[CreateAssetMenu(fileName = "MoneyOutcome", menuName = "ScriptableObjects/Missions/Outcomes/MoneyOutcome", order = 1)]
public class MoneyOutcome : MissionOutcome
{
    [SerializeField] private float moneyMin;
    [SerializeField] private float moneyMax;

    public new MoneyOutcomeSaveData saveData;

    [System.Serializable]
    public class MoneyOutcomeSaveData : MissionOutcomeSaveData
    {
        public float moneyReceived;
    }

    public override void Process(Mission mission)
    {
        long moneyReceived = (long)Random.Range(moneyMin, moneyMax);
        PlayerManager.Instance.ReceiveMoney(moneyReceived);
    }
}