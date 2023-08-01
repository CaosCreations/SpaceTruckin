
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeAppearance : MonoBehaviour
{
    [SerializeField] private Material normalSpeedMaterial;
    [SerializeField] private Material slowSpeedMaterial;

    private MeshRenderer meshRenderer;

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.material = normalSpeedMaterial;
    }

    public void SetNormalSpeedMaterial()
    {
        meshRenderer.material = normalSpeedMaterial;
    }

    public void SetSlowSpeedMaterial()
    {
        meshRenderer.material = slowSpeedMaterial;
    }

}