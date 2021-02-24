using UnityEngine;
using System.Linq;

public enum LicenceEffectType
{
    Money, Discount, Xp, ShipDamage
}

public class LicencesManager : MonoBehaviour, IDataModelManager
{
    public static LicencesManager Instance { get; private set; }

    public MoneyEffect[] moneyEffects;
    public PilotXpEffect[] pilotXpEffects;
    public ShipDamageEffect[] shipDamageEffects;

    public static double MoneyEffect => Instance.moneyEffects.ToList().Sum(x => x.effect);
    public static double PilotXpEffect => Instance.pilotXpEffects.ToList().Sum(x => x.effect);
    public static float ShipDamageEffect => Instance.shipDamageEffects.ToList().Sum(x => x.effect);

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
