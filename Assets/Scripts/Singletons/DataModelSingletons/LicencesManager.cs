using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LicencesManager : MonoBehaviour, IDataModelManager
{
    public static LicencesManager Instance { get; private set; }

    [SerializeField] private LicenceContainer licenceContainer;
    public Licence[] Licences => licenceContainer.Elements;

    // Total effect getters 
    public static double MoneyEffect => GetTotalPercentageEffect<MoneyEffect>();
    public static double PilotXpEffect => GetTotalPercentageEffect<PilotXpEffect>();
    public static double ShipDamageEffect => GetTotalPercentageEffect<ShipDamageEffect>();
    public static double FuelDiscountEffect => GetTotalPercentageEffect<FuelDiscountEffect>();
    public static double HangarSlotUnlockEffect => GetTotalHangarSlotEffect();

    public static int NumberOfTiers => Instance.Licences.Max(x => x.Tier);

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void Init()
    {
        if (DataUtils.SaveFolderExists(Licence.FolderName))
        {
            LoadDataAsync();
        }
        else
        {
            DataUtils.CreateSaveFolder(Licence.FolderName);
        }

        if (Licences == null)
        {
            Debug.LogError("No licences data");
        }
    }

    /// <summary>
    /// Key is the tier.
    /// Value is a list of the licences on that tier.
    /// </summary>
    /// <returns></returns>
    public static Dictionary<int, List<Licence>> GetLicencesGroupedByTiers()
    {
        var licenceGroups = new Dictionary<int, List<Licence>>();
        for (int i = 1; i <= NumberOfTiers; i++)
        {
            licenceGroups.Add(i, new List<Licence>());
            Instance.Licences.Where(x => x.Tier == i).ToList().ForEach(y => licenceGroups[i].Add(y));
        }
        return licenceGroups;
    }

    private static List<T> GetActiveLicenceEffectsByType<T>() where T : LicenceEffect
    {
        var effects = new List<T>();
        Instance.Licences
            .Where(x => x.Effect is T && x.IsOwned)
            .ToList()
            .ForEach(y => effects.Add(y.Effect as T));

        return effects;
    }

    public static double GetTotalPercentageEffect<T>() where T : PercentageEffect
    {
        return GetActiveLicenceEffectsByType<T>().Sum(x => x.Coefficient);
    }

    public static int GetTotalHangarSlotEffect()
    {
        return GetActiveLicenceEffectsByType<HangarSlotUnlockEffect>().Sum(x => x.NumberOfSlotsToUnlock);
    }

    #region Persistence
    public void SaveData()
    {
        foreach (Licence licence in Instance.Licences)
        {
            if (licence != null)
            {
                licence.SaveData();
            }
        }
    }

    public async void LoadDataAsync()
    {
        foreach (Licence licence in Instance.Licences)
        {
            if (licence != null)
            {
                await licence.LoadDataAsync();
            }
        }
    }

    public void DeleteData()
    {
        Instance.DeleteData();
    }
    #endregion
}
