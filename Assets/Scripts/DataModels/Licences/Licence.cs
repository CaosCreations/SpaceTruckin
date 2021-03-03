using System;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Licence", menuName = "ScriptableObjects/Licences/Licence", order = 1)]
public partial class Licence : ScriptableObject, IDataModel
{
    [Header("Set in Editor")]
    [SerializeField] private string licenceName;
    [SerializeField] private string description;
    [SerializeField] private int tier;
    [SerializeField] private int pointsCost;
    [SerializeField] private LicenceEffect effect;

    // Owning at least one antecedent is a necessary condition to buy the licence
    [SerializeField] private Licence[] antecedentLicences;

    [Header("Data to update IN GAME")]
    public LicenceSaveData saveData;

    public static string FOLDER_NAME = "LicenceSaveData";

    [Serializable]
    public class LicenceSaveData
    {
        // Some licences become available through external factors
        public bool isUnlocked; 

        public bool isOwned;
    }

    public void SaveData()
    {
        DataUtils.SaveFileAsync(name, FOLDER_NAME, saveData);
    }

    public async Task LoadDataAsync()
    {
        saveData = await DataUtils.LoadFileAsync<LicenceSaveData>(name, FOLDER_NAME);
    }
}
