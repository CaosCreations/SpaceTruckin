using UnityEngine;
using System.Linq;

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

    public static void InvestLicencePoint(Licence licence)
    {
        licence.PointsInvested++;
        PlayerManager.Instance.InvestLicencePoint();
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
