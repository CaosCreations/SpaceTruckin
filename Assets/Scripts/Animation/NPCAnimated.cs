using UnityEngine;

public class NPCAnimated : MonoBehaviour
{
    [SerializeField]
    private Animator animator;

    public Animator Animator => animator;

    public void SetBoolAfterReset(string parameterName, bool value)
    {
        animator.ResetBools();
        animator.SetBool(parameterName, value);
    }
}
