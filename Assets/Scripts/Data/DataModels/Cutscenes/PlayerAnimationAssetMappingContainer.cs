using UnityEngine;

[CreateAssetMenu(fileName = "PlayerAnimationAssetMappingContainer", menuName = "ScriptableObjects/PlayerAnimationAssetMappings/PlayerAnimationAssetMappingContainer", order = 1)]
public class PlayerAnimationAssetMappingContainer : ScriptableObject, IScriptableObjectContainer<PlayerAnimationAssetMapping>
{
    [field: SerializeField]
    public PlayerAnimationAssetMapping[] Elements { get; set; }

    public PlayerAnimationAssetMapping GetMappingByClipName(string clipName)
    {
        foreach (var mapping in Elements)
        {
            if (clipName == mapping.Player1Clip.name || clipName == mapping.Player2Clip.name)
            {
                return mapping;
            }
        }
        Debug.LogWarning("Mapping not found with clip name: " + clipName);
        return null;
    }
}