using UnityEngine;

public class ShipsManager : MonoBehaviour
{
    public static ShipsManager Instance;

    public Ship[] ships;

    void Awake()
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

    public static void DamageShip(int index, int damage)
    {
        for (int i = 0; i < Instance.ships.Length; i++)
        {
            if (Instance.ships[i] != null && index == Instance.ships[i].id)
            {
                Instance.ships[i].hullIntegrity = Mathf.Max(0, Instance.ships[i].hullIntegrity - damage);
            }
        }
    }
}
