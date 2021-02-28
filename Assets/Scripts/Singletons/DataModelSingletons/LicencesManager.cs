using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class LicencesManager : MonoBehaviour, IDataModelManager
{
    public static LicencesManager Instance { get; private set; }

    [SerializeField] private LicenceContainer licenceContainer;
    public Licence[] Licences { get => licenceContainer.licences; }

    public List<MoneyEffect> moneyEffects;
    public List<PilotXpEffect> pilotXpEffects;
    public List<ShipDamageEffect> shipDamageEffects;
    public List<FuelDiscountEffect> fuelDiscountEffects;

    public static double MoneyEffect => Instance.moneyEffects.Sum(x => x.Effect);
    public static double PilotXpEffect => Instance.pilotXpEffects.Sum(x => x.Effect);
    public static double ShipDamageEffect => Instance.shipDamageEffects.Sum(x => x.Effect);
    public static double FuelDiscountEffect => Instance.fuelDiscountEffects.Sum(x => x.Effect);
    public static int NumberOfTiers => Instance.Licences.Max(x => x.Tier);

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    public void Init()
    {
        if (DataUtils.SaveFolderExists(Message.FOLDER_NAME))
        {
            LoadDataAsync();
            SetEffectsReferences();
        }
        else
        {
            DataUtils.CreateSaveFolder(Message.FOLDER_NAME);
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

    private static List<T> GetLicenceEffectsByType<T>() where T : LicenceEffect
    {
        var effects = new List<T>();
        Instance.Licences.Where(x => x.Effect is T)
            .ToList()
            .ForEach(y => effects.Add(y.Effect as T));
        
        return effects;
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

    // Todo: store only owned effects
    private void SetEffectsReferences()
    {
        moneyEffects = GetLicenceEffectsByType<MoneyEffect>();
        pilotXpEffects = GetLicenceEffectsByType<PilotXpEffect>();
        shipDamageEffects = GetLicenceEffectsByType<ShipDamageEffect>();
        fuelDiscountEffects = GetLicenceEffectsByType<FuelDiscountEffect>();
    }

    public static double GetTotalEffect<T>() where T : LicenceEffect
    {
        return GetActiveLicenceEffectsByType<T>().Sum(x => x.Effect);
        // Todo: don't use LINQ every time
    }


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
}
