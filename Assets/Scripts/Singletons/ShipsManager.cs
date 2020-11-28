using UnityEngine;

public class ShipsManager : MonoBehaviour
{
    public static ShipsManager Instance;

    private static Ship[] ships;

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
        for (int i = 0; i < ships.Length; i++)
        {
            if (ships[i] != null && index == ships[i].id)
            {
                ships[i].hullIntegrity = Mathf.Max(0, ships[i].hullIntegrity - damage);
            }
        }
    }
}
