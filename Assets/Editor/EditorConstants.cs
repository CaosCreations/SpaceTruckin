using UnityEditor;

public static class EditorConstants
{
    // Asset bundling 
    public const string AssetBundleDirectory = "Assets/StreamingAssets";
    public const BuildAssetBundleOptions AssetBundleOptions = BuildAssetBundleOptions.None;
    public static readonly BuildTarget AssetBundleBuildTarget = EditorUserBuildSettings.activeBuildTarget;

    public const string ImportedMissionsPath = "Assets/ScriptableObjects/Missions/ImportedMissions";

    // Scriptable object importing 
    public static readonly string[] MissionImportPropertyKeys = new string[]
    {
        "Name",
        "Customer",
        "Cargo",
        "Description",
        "DurationInDays",
        "FuelCost",
        "UnlockCondition",
        "MoneyNeededToUnlock",
        "IsRepeatable",
        "HasRandomOutcomes",
        "FondnessGranted",
        "OfferTimeLimitInDays",
        "OfferExpiryFondnessDeduction",
    };
}
