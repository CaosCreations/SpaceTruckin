using UnityEngine;

public class InputAnimationTrigger : MonoBehaviour
{
    [SerializeField]
    private AnimationByInput[] animationsByInputs;

    private void Update()
    {
        foreach (var inputMap in animationsByInputs)
        {
            if (Input.GetKeyDown(inputMap.KeyCode)
                || (!string.IsNullOrWhiteSpace(inputMap.ButtonName) && Input.GetButtonDown(inputMap.ButtonName)))
            {
                inputMap.Animator.PlayOnce(inputMap.PlayClipName, inputMap.ResetClipName);
            }
        }
    }
}
