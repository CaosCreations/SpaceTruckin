using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public static class DataModelsUtils
{
    public const string FILE_EXTENSION = ".truckin";

    public static async void SaveFileAsync<T>(string fileName, string folderName, T dataModel)
    {
        string folderPath = GetFullPath(folderName);
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

        string filePath = Path.Combine(folderPath, fileName + FILE_EXTENSION);
        string fileContents = JsonUtility.ToJson(dataModel);

        try
        {
            var buffer = Encoding.UTF8.GetBytes(fileContents);
            
            using (var fileStream = new FileStream(filePath, FileMode.OpenOrCreate,
                FileAccess.Write, FileShare.None, buffer.Length, true))
            {
                await fileStream.WriteAsync(buffer, 0, buffer.Length);
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"{e.Message}\n{e.StackTrace}");
        }
    }

    public static async Task<T> LoadFileAsync<T>(string fileName, string folderName) where T : class, new()
    {
        string folderPath = GetFullPath(folderName);
        string filePath = Path.Combine(folderPath, fileName + FILE_EXTENSION);

        if (File.Exists(filePath))
        {
            try
            {
                string json;
                using (StreamReader reader = File.OpenText(filePath))
                {
                    json = await reader.ReadToEndAsync();
                }
                return JsonUtility.FromJson<T>(json);
            }
            catch (Exception e)
            {
                Debug.LogError($"{e.Message}\n{e.StackTrace}");
            }
        }
        else
        {

        }

        return new T();
    }

    public static string GetFullPath(string name)
    {
        return Path.GetFullPath(Path.Combine(Application.persistentDataPath, name));
    }

    public static void RecursivelyDeleteSaveData(string folderName)
    {
        string folderPath = GetFullPath(folderName);
        if (Directory.Exists(folderPath))
        {
            Directory.Delete(folderPath, recursive: true);
        }
    }

    public static bool SaveFolderExists(string folderName)
    {
        string folderPath = GetFullPath(folderName);
        if (Directory.Exists(folderPath)) 
        {
            // Get entries so we can confirm that the folder is not empty
            IEnumerable<string> entries = Directory.EnumerateFileSystemEntries(folderPath);

            if (entries != null && entries.Any())
            {
                return true;
            }
        }
        return false; 
    }

    public static bool SaveFileExists(string fileName)
    {
        string filePath = GetFullPath(fileName);
        return File.Exists(filePath);
    }

    public static void CreateSaveFolder(string folderName)
    {
        Directory.CreateDirectory(GetFullPath(folderName));
    }
}