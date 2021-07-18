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

    public void PlayAnimation(NPCAnimated nPCAnimated, NPCAnimationParameterType npcAnimationParameterType, bool isOn)
    {
        if (nPCAnimated.ParameterMap.ContainsKey(npcAnimationParameterType))
        {
            nPCAnimated.Animator.SetBool(nPCAnimated.ParameterMap[npcAnimationParameterType], isOn);
        }
        else
        {
            Debug.LogError("The NPCAnimationParameterType is missing from the ParameterMap dictionary. Please add it.");
        }
    }
}
