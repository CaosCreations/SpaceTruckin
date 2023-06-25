using UnityEngine;

public class BatteryCharging : MonoBehaviour
{
    public bool IsCharged { get; set; }

    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private float emissionCoefficient = 1.0f;
    private Color emission;

    private void Awake()
    {
        Init();
    }

    public void Init()
    {
        if (meshRenderer == null)
        {
            Debug.Log(
                $"Getting MeshRenderer on battery {gameObject.name} as the reference is missing on the prefab.");

            meshRenderer = GetComponent<MeshRenderer>();
        }

        if (meshRenderer != null)
        {
            meshRenderer.material.EnableKeyword("_EMISSION");
            emission = meshRenderer.material.GetColor("_EmissionColor");
        }
        else
        {
            Debug.LogError($"MeshRenderer on battery {gameObject.name} is null");
        }
    }

    public void Charge()
    {
        IsCharged = true;
        SetEmission();
    }

    public void Discharge()
    {
        IsCharged = false;
        SetEmission();
    }

    private void SetEmission()
    {
        Color e = IsCharged ? emission * emissionCoefficient : emission;
        meshRenderer.material.SetColor("_EmissionColor", e);
    }

    public void LoadData(BatterySaveData saveData)
    {
        IsCharged = saveData.IsCharged;
        SetEmission();
    }
}
