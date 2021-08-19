using UnityEngine;

public class NPCAnimationManager : MonoBehaviour
{
    public static NPCAnimationManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    public void PlayAnimation(NPCAnimated npcAnimated, NPCAnimationParameterType npcAnimationParameterType, bool isOn)
    {
        if (npcAnimated.ParameterMap.ContainsKey(npcAnimationParameterType))
        {
            npcAnimated.Animator.SetBool(npcAnimated.ParameterMap[npcAnimationParameterType], isOn);
        }
        else
        {
            Debug.LogError($"The NPCAnimationParameterType '{nameof(npcAnimationParameterType)}' is missing from the ParameterMap dictionary. Please add it.");
        }
    }
}
