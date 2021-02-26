using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class LicencesManager : MonoBehaviour, IDataModelManager
{
    public static LicencesManager Instance { get; private set; }

    [SerializeField] private LicenceContainer licenceContainer;
    public Licence[] Licences { get => licenceContainer.licences; }

    public MoneyEffect[] moneyEffects;
    public PilotXpEffect[] pilotXpEffects;
    public ShipDamageEffect[] shipDamageEffects;
    public FuelDiscountEffect[] fuelDiscountEffects;

    public static double MoneyEffect => Instance.moneyEffects.ToList().Sum(x => x.effect);
    public static double PilotXpEffect => Instance.pilotXpEffects.ToList().Sum(x => x.effect);
    public static float ShipDamageEffect => Instance.shipDamageEffects.ToList().Sum(x => x.effect);
    public static float FuelDiscountEffect => Instance.fuelDiscountEffects.ToList().Sum(x => x.effect);

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
        if (DataModelsUtils.SaveFolderExists(Message.FOLDER_NAME))
        {
            LoadDataAsync();
        }
        else
        {
            DataModelsUtils.CreateSaveFolder(Message.FOLDER_NAME);
        }

        if (Licences == null)
        {
            Debug.LogError("No licences data");
        }
    }

    public static Licence[] GetLicencesInTierOrder()
    {
        return Instance.Licences.OrderBy(x => x.Tier).ToArray();
    }

    /// <summary>
    /// Key is the tier.
    /// Value is a list of the licences on that tier.
    /// </summary>
    /// <returns></returns>
    public static Dictionary<int, List<Licence>> GetLicencesGroupedByTiers()
    {
        var licenceGroups = new Dictionary<int, List<Licence>>();
        for (int i = 1; i <= LicenceConstants.NumberOfTiers; i++)
        {
            licenceGroups.Add(i, new List<Licence>());
            Instance.Licences.Where(x => x.Tier == i).ToList().ForEach(y => licenceGroups[i].Add(y));
            //Instance.Licences.Where(x => x.Tier == i).ToList().ForEach(y => licenceGroups.Add(i, y));
        }
        return licenceGroups;
    }

    private static IEnumerable<LicenceEffect> GetLicenceEffectsByType<T>() 
    {
        var effects = new List<LicenceEffect>();
        Instance.Licences.Where(x => x.Effect is T).ToList().ForEach(y => effects.Add(y.Effect));
        return effects;
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
