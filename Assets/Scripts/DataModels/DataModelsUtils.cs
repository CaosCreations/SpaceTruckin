
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public static class DataModelsUtils
{
    public static void SaveAllData(IDataModel[] dataModels)
    {
        foreach (IDataModel dataModel in dataModels)
        {
            dataModel.SaveData();
        }
    }

    public static void SaveData(IDataModel model)
    {
        model.SaveData();
    }

    public static async void SaveFileAsync<T>(string fileName, string folderName, T dataModel)
    {
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
        string filePath = Path.Combine(folderPath, fileName);
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
        string folderPath = Path.Combine(Application.persistentDataPath, folderName);
        string filePath = Path.Combine(folderPath, fileName);

        if (File.Exists(filePath))
        {
            try
            {
                // Create async file stream 
                using (var sourceStream =
                    new FileStream(
                        filePath,
                        FileMode.Open, FileAccess.Read, FileShare.Read,
                        bufferSize: 4096, useAsync: true))
                {
                    var builder = new StringBuilder();

                    // Create a byte array of size 4096 = 0x1000
                    byte[] buffer = new byte[0x1000];

                    // Read bytes asynchronously until the end of the file is reached 
                    int numberOfBytesRead;
                    while ((numberOfBytesRead = await sourceStream.ReadAsync(buffer, 0, buffer.Length)) != 0)
                    {
                        string text = Encoding.Unicode.GetString(buffer, 0, numberOfBytesRead);
                        builder.Append(text);
                    }
                    // Deserialise the string
                    return JsonUtility.FromJson<T>(builder.ToString());
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"{e.Message}\n{e.StackTrace}");
            }
        }
        return new T();
    }

    public static string GetUniqueFileName(string fileName, Guid guid)
    {
        return $"{fileName}_{guid}.truckin";
    }
}