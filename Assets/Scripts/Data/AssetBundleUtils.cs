using System.IO;
using UnityEngine;

public static class AssetBundleUtils
{
    public static TextAsset LoadTextAsset(AssetBundle bundle, string assetPath)
    {
        if (bundle == null)
        {
            Debug.LogError("Failed to load Asset Bundle");
            return null;
        }

        TextAsset textAsset = bundle.LoadAsset(PilotsConstants.HumanMaleNamesPath) as TextAsset;

        return textAsset;

    }
}
