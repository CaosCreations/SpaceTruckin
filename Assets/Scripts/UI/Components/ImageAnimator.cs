using UnityEngine;

public class ImageAnimator : MonoBehaviour
{
    [SerializeField] private bool isLooping;
    [SerializeField] private string loopStateName;
    [SerializeField] private Sprite staticSprite;
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer spriteRenderer;

    private void Start()
    {
        UpdateAnimation();
    }

    public void UpdateAnimation(bool isLooping)
    {
        this.isLooping = isLooping;
        UpdateAnimation();
    }

    private void UpdateAnimation()
    {
        if (isLooping)
        {
            animator.enabled = true;
            if (!string.IsNullOrWhiteSpace(loopStateName))
            {
                animator.Play(loopStateName);
            }
        }
        else
        {
            animator.enabled = false;
            spriteRenderer.sprite = staticSprite;
        }
    }
}
