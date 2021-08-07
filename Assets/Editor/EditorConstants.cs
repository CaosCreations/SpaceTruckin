using UnityEditor;

public static class EditorConstants
{
    // Asset bundling 
    public const string AssetBundleDirectory = "Assets/StreamingAssets";
    public const BuildAssetBundleOptions AssetBundleOptions = BuildAssetBundleOptions.None;
    public static readonly BuildTarget AssetBundleBuildTarget = EditorUserBuildSettings.activeBuildTarget;

    public const string ImportedMissionsPath = "Assets/ScriptableObjects/Missions/ImportedMissions";
}
