using UnityEngine;

public class CamAnimStateChange : MonoBehaviour
{
    [SerializeField] private Animator animator;

    public void PlayState(string stateName)
    {
        animator.Play(stateName, 0, 0);
    }
}
