using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class EditorHelper : MonoBehaviour
{
    public static T GetAsset<T>() where T : class
    {
        try
        {
            var asset = GetAsset(typeof(T).ToString());
            return asset as T;
        }
        catch (Exception ex)
        {
            Debug.LogError($"{ex.Message}\n{ex.StackTrace}");
            return null;
        }
    }

    public static UnityEngine.Object GetAsset(string filter)
    {
        try
        {
            var guid = AssetDatabase.FindAssets(filter).FirstOrDefault();
            var path = AssetDatabase.GUIDToAssetPath(guid);
            var asset = AssetDatabase.LoadMainAssetAtPath(path);
            return asset;
        }
        catch (Exception ex)
        {
            Debug.LogError($"{ex.Message}\n{ex.StackTrace}");
            return null;
        }
    }
}
