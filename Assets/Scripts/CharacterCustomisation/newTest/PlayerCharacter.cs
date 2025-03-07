using UnityEngine;

public class PlayerCharacter : MonoBehaviour
{
    public SpriteRenderer bodyRenderer;
    public SpriteRenderer hairRenderer;
    public SpriteRenderer clothesRenderer;


    public Animator animator;

    public void SetupCharacter(CharacterData data)
    {
        CharacterCustomizer customizer = FindObjectOfType<CharacterCustomizer>();

        if (customizer != null)
        {
            bodyRenderer.sprite = customizer.bodySprites[data.bodyIndex];
            hairRenderer.sprite = customizer.hairSprites[data.hairIndex];
            clothesRenderer.sprite = customizer.clothesSprites[data.clothesIndex];
        }
    }

    private void Update()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        animator.SetFloat("MoveX", moveX);
        animator.SetFloat("MoveY", moveY);
        animator.SetBool("IsMoving", moveX != 0 || moveY != 0);
    }
}
