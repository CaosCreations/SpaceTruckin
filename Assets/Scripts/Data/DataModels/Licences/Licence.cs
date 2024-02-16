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

    public const string FolderName = "LicenceSaveData";

    [Serializable]
    public class LicenceSaveData
    {
        public bool isUnlocked; //Some become available through external factors
        public bool isOwned;
    }

    public void SaveData()
    {
        DataUtils.SaveFileAsync(name, FolderName, saveData);
    }

    public async Task LoadDataAsync()
    {
        saveData = await DataUtils.LoadFileAsync<LicenceSaveData>(name, FolderName);
    }

    public void ResetData()
    {
        saveData = new();
    }
}
