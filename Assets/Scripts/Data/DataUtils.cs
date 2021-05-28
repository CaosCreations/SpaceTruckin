using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public static class DataUtils
{
    public const string FILE_EXTENSION = ".truckin";

    public static async void SaveFileAsync<T>(string fileName, string folderName, T dataModel)
    {
        string folderPath = GetSaveFolderPath(folderName);
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
                fileStream.SetLength(0);
                await fileStream.WriteAsync(buffer, 0, buffer.Length);
            }
        }
        catch (Exception e)
        {
            LogIOError(e, filePath);
        }
    }

    public static async void SaveFileAsync(string fileName, string folderName, string fileContents)
    {
        string folderPath = GetSaveFolderPath(folderName);
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
        try
        {
            var buffer = Encoding.UTF8.GetBytes(fileContents);

            using (var fileStream = new FileStream(filePath, FileMode.OpenOrCreate,
                FileAccess.Write, FileShare.None, buffer.Length, true))
            {
                fileStream.SetLength(0);
                await fileStream.WriteAsync(buffer, 0, buffer.Length);
            }
        }
        catch (Exception e)
        {
            LogIOError(e, filePath);
        }
    }

    public static async Task<T> LoadFileAsync<T>(string fileName, string folderName) where T : class, new()
    {
        string folderPath = GetSaveFolderPath(folderName);
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
                LogIOError(e, filePath);
            }
        }
        return new T();
    }

    public static string GetSaveFolderPath(string folderName)
    {
        return Path.GetFullPath(Path.Combine(Application.persistentDataPath, folderName));
    }

    public static string GetSaveFilePath(string folderName, string fileName)
    {
        return Path.GetFullPath(Path.Combine(Application.persistentDataPath, folderName, fileName + FILE_EXTENSION));
    }

    public static void RecursivelyDeleteSaveData(string folderName)
    {
        string folderPath = GetSaveFolderPath(folderName);
        if (Directory.Exists(folderPath))
        {
            Directory.Delete(folderPath, recursive: true);
        }
    }

    public static bool SaveFolderExists(string folderName)
    {
        string folderPath = GetSaveFolderPath(folderName);
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

    public static bool IsNewGame()
    {
        return !SaveFolderExists(PlayerData.FOLDER_NAME);
    }

    public static void CreateSaveFolder(string folderName)
    {
        Directory.CreateDirectory(GetSaveFolderPath(folderName));
    }

    public async static Task<string> ReadFileAsync(string filePath)
    {
        string text = string.Empty;
        if (File.Exists(filePath))
        {
            using (StreamReader reader = File.OpenText(filePath))
            {
                text = await reader.ReadToEndAsync();
            }
        }
        else
        {
            Debug.Log($"File at path {filePath} does not exist");
        }
        return text;
    }

    public static void LogIOError(Exception e, string filePath)
    {
        Debug.LogError($"Message: {e.Message}\nTrace: {e.StackTrace}\nFile path: {filePath}");
    }
}
