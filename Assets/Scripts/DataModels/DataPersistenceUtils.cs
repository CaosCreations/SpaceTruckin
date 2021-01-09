using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public enum PersistentDataType
{
    Mission = 1, Pilot = 2, Ship = 3, Message = 4 
}

public interface IPersistentData
{
    string folderName;
    void SaveData();
    void LoadData();

}

public static class DataPersistenceUtils
{
    private static void SaveData(object persistentData, string folderName, string fileName)
    {
        Debug.Log($"Saving file: {fileName}");
        string folderPath = Path.Combine(Application.persistentDataPath, folderName);
        if (!Directory.Exists(folderPath))
        {
            try
            {
                Directory.CreateDirectory(folderPath);
            }
            catch (Exception e)
            {
                Debug.LogError($"{e.Message}\n{e.StackTrace}");
            }
        }

        string filePath = Path.Combine(folderPath, $"{fileName}.save");
        string fileContents = JsonUtility.ToJson(persistentData);
        try
        {
            File.WriteAllText(filePath, fileContents);
        }
        catch (Exception e)
        {
            Debug.LogError($"{e.Message}\n{e.StackTrace}");
        }
        Debug.Log($"Finished saving file: {fileName}");

    }

    private static void LoadData(string folderName, string fileName, PersistentDataType type)
    {
        Debug.Log($"Loading file: {fileName}");
        string folderPath = Path.Combine(Application.persistentDataPath, folderName);
        string filePath = Path.Combine(folderPath, $"{fileName}.save");
        if (File.Exists(filePath))
        {
            try
            {
                string fileContents = File.ReadAllText(filePath);
                object persistentData = JsonUtility.FromJson<object>(fileContents);
            }
            catch (Exception e)
            {
                Debug.LogError($"{e.Message}\n{e.StackTrace}");
            }
        }
        Debug.Log($"Finished loading file:{fileName}");
    }

    //private static ScriptableObject GetScriptableObjectType(PersistentDataType persistentDataType)
    //{
    //    switch (persistentDataType)
    //    {
    //        case PersistentDataType.Mission:
    //            return 
    //    }

    //}

    //private static string GetFileName(object )
    //{
        // 
    //}
}

