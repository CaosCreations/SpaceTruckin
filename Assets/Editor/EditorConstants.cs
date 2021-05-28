using UnityEditor;

public static class EditorConstants
{
    public const string AssetBundleDirectory = "Assets/StreamingAssets";
    public const BuildAssetBundleOptions AssetBundleOptions = BuildAssetBundleOptions.None;
    public static BuildTarget AssetBundleBuildTarget = EditorUserBuildSettings.activeBuildTarget;
}
