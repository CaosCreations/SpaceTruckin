using UnityEngine;

public class ShipsManager : MonoBehaviour
{
    public static ShipsManager Instance;

    public Ship[] ships;

    private void Awake()
    {
        if (FindObjectsOfType(GetType()).Length > 1)
        {
            Destroy(gameObject);
        }

        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance == this)
        {
            Destroy(Instance.gameObject);
            Instance = this;
        }
        DontDestroyOnLoad(this.gameObject);
    }

    public static void DamageShip(int shipId, int damage)
    {
        ships[shipId].hullIntegrity = Mathf.Max(0, ships[shipId].hullIntegrity - damage);
    }
}
