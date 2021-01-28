using UnityEngine;

[CreateAssetMenu(fileName = "MoneyOutcome", menuName = "ScriptableObjects/Missions/Outcomes/MoneyOutcome", order = 1)]
public class MoneyOutcome : MissionOutcome
{
    [SerializeField] private long moneyMin;
    [SerializeField] private long moneyMax;

    //public MoneyOutcomeSaveData moneySaveData;

    //[System.Serializable]
    //public class MoneyOutcomeSaveData : MissionOutcomeSaveData
    //{
    //    public long moneyReceived;
    //}

    //public long MoneyReceived
    //{
    //    get => moneySaveData.moneyReceived; set => moneySaveData.moneyReceived = value;
    //}

    public override void Process(Mission mission)
    {
        long moneyEarned = (long)Random.Range(moneyMin, moneyMax);
        PlayerManager.Instance.ReceiveMoney(moneyEarned);

        if (mission.MissionToArchive != null)
        {
            mission.MissionToArchive.TotalMoneyEarned += moneyEarned;
            Debug.Log("MTA Money: " + mission.MissionToArchive.TotalMoneyEarned);
        }
    }
}