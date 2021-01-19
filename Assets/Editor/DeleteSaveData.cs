using UnityEditor;
using UnityEngine;
using System.IO;

public class DeleteSaveData : MonoBehaviour
{
    [MenuItem("Space Truckin/Delete Save Data")]
    private static void DeleteData() 
    {
        if (!Directory.Exists(Application.persistentDataPath))
        {
            Debug.Log("No save data to delete");
            return;
        }
        Directory.Delete(Application.persistentDataPath, recursive: true);
        Debug.Log("Save data deleted");
    }
}
