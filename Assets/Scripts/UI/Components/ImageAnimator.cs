using UnityEngine;
using UnityEngine.UI;

public class ImageAnimator : MonoBehaviour
{
    [SerializeField] private bool isLooping;
    [SerializeField] private string loopStateName = "LoopAnimation";
    [SerializeField] private Sprite staticSprite; 
    private Animator animator;
    private Image image;

    private void Start()
    {
        animator = GetComponent<Animator>();
        image = GetComponent<Image>();
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
            animator.Play(loopStateName);
        }
        else
        {
            animator.enabled = false;
            image.sprite = staticSprite;
        }
    }
}
