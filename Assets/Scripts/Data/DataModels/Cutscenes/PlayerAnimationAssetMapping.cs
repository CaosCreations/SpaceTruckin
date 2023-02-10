using UnityEngine;

[CreateAssetMenu(fileName = "PlayerAnimationAssetMapping", menuName = "ScriptableObjects/PlayerAnimationAssetMappings/PlayerAnimationAssetMapping", order = 1)]
public class PlayerAnimationAssetMapping : ScriptableObject
{
    public string AnimationAssetName;
    public AnimationClip Player1Clip;
    public AnimationClip Player2Clip;
}
