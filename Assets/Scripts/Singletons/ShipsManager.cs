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
