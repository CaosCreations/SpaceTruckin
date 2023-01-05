using UnityEngine;

[CreateAssetMenu(fileName = "PlayerAnimationAssetMappingContainer", menuName = "ScriptableObjects/PlayerAnimationAssetMappings/PlayerAnimationAssetMappingContainer", order = 1)]
public class PlayerAnimationAssetMappingContainer : ScriptableObject, IScriptableObjectContainer<PlayerAnimationAssetMapping>
{
    [field: SerializeField]
    public PlayerAnimationAssetMapping[] Elements { get; set; }
}