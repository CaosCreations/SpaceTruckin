using System.Collections;
using System.IO;
using UnityEngine;

public static class AssetBundleUtils
{
    public static IEnumerator LoadAssetBundleAsync(AssetBundle bundle, string bundlePath, string bundleName)
    {
        AssetBundleCreateRequest bundleRequest = AssetBundle.LoadFromFileAsync(
            Path.Combine(bundlePath, bundleName));

        yield return bundleRequest;

        bundle = bundleRequest.assetBundle;
    }

    public static IEnumerator LoadAssetAsync(AssetBundle bundle, string assetPath)
    {
        if (bundle == null)
        {
            Debug.LogError("Failed to load Asset Bundle");
            yield break;
        }

        AssetBundleRequest assetRequest = bundle.LoadAssetAsync(assetPath);

        yield return assetRequest;
    }
}
