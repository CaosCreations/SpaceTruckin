using UnityEngine;

public class PlayerCustomisation : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void SetSpriteRendererColour(Color newColour)
    {
        spriteRenderer.color = newColour;
    }
}
