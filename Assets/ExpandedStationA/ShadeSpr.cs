using UnityEngine;

public class ShadeSpr : MonoBehaviour
{
    private Renderer _renderer;
    private bool shadowOnly;

    private void Start()
    {
        if (_renderer == null)
        {
            Debug.Log("Renderer is empty");

            _renderer = GetComponent<Renderer>();
        }

        _renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
        _renderer.receiveShadows = true;

        if (shadowOnly)
        {
            _renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly;
        }
    }
}
