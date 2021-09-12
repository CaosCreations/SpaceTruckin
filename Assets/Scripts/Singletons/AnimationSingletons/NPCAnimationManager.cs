using UnityEngine;

public class NPCAnimationManager : AnimationManager<NPCAnimationParameterType>
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
        if (!npcAnimated.ParameterMap.ContainsKey(npcAnimationParameterType))
        {
            // There is no mapping at all
            LogMissingParameterMapping(npcAnimationParameterType);
        }
        else if (!npcAnimated.Animator.ContainsParameterWithName(npcAnimated.ParameterMap[npcAnimationParameterType]))
        {
            // There is a key but not a value that matches a parameter on the Animator 
            Debug.LogError($"Animation parameter with name '{npcAnimated.ParameterMap[npcAnimationParameterType]} does not exist on {nameof(NPCAnimated)}.");
        }
        else
        {
            npcAnimated.Animator.SetBool(npcAnimated.ParameterMap[npcAnimationParameterType], isOn);
        }
    }
}
