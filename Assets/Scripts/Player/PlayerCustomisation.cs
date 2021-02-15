using UnityEngine;

public class PlayerCustomisation : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    public void Init(Color newColour)
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null && spriteRenderer.material != null)
        {
            SetSpriteRendererColour(newColour);
        }
    }

    public void SetSpriteRendererColour(Color newColour)
    {
        spriteRenderer.material.color = newColour;
        Debug.Log($"Player sprite colour set to: {newColour}");
    }
}
