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
    [SerializeField] private Licence prerequisiteLicence;
    [SerializeField] private LicenceEffect effect;

    [Header("Data to update IN GAME")]
    public LicenceSaveData saveData;

    public static string FOLDER_NAME = "LicenceSaveData";

    [Serializable]
    public class LicenceSaveData
    {
        public bool isUnlocked; //Some become available through external factors
        public bool isOwned;
    }

    public void SaveData()
    {
        DataModelsUtils.SaveFileAsync(name, FOLDER_NAME, saveData);
    }

    public async Task LoadDataAsync()
    {
        saveData = await DataModelsUtils.LoadFileAsync<LicenceSaveData>(name, FOLDER_NAME);
    }
}
