using System.IO;
using UnityEditor;
using UnityEngine;

public class CreateAssetBundles 
{
    [MenuItem("Assets/Build Asset Bundles")]
    private static void BuildAssetBundles()
    {
        if (!Directory.Exists(Application.streamingAssetsPath))
        {
            Directory.CreateDirectory(EditorConstants.AssetBundleDirectory);
        }

        BuildPipeline.BuildAssetBundles(
            EditorConstants.AssetBundleDirectory,
            EditorConstants.AssetBundleOptions,
            EditorConstants.AssetBundleBuildTarget);
    }
}
