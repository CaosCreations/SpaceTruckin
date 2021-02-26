using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public enum LicenceEffectType
{
    Money, Discount, Xp, ShipDamage
}

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

    public static Licence[] GetLicencesInTierOrder()
    {
        return Instance.Licences.OrderBy(x => x.Tier).ToArray();
    }

    public static Dictionary<int, Licence> GetLicencesGroupedByTiers()
    {
        // Needs to be 2D
        // <int, List<Licence>> 

        var licenceGroups = new Dictionary<int, Licence>();
        for (int i = 1; i <= LicenceConstants.NumberOfTiers; i++)
        {
            Instance.Licences.Where(x => x.Tier == i).ToList().ForEach(y => licenceGroups.Add(i, y));
        }
        return licenceGroups;
    }

    public static bool IsTierUnlocked(Licence licence)
    {
        return PlayerManager.Instance.TotalLicencePointsAcquired
            >= Instance.PointRequirementsPerTier[key: licence.Tier];
    }

    public void DeleteData()
    {
        throw new System.NotImplementedException();
    }

    public void Init()
    {
        throw new System.NotImplementedException();
    }

    public void LoadDataAsync()
    {
        throw new System.NotImplementedException();
    }

    public void SaveData()
    {
        throw new System.NotImplementedException();
    }
}
