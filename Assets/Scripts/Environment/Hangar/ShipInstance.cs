using UnityEngine;

public class ShipInstance : MonoBehaviour
{
    [SerializeField] private ShipLaunch launch;

    public void Launch()
    {
        launch.Launch();
    }
}
