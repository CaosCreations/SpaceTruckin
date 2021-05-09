using UnityEngine;

public static class ObjectExtensions
{
    public static void LogErrorIfNull(this Object self, string objectDescription = null)
    {
        if (self != null)
        {
            return;
        }

        string textToLog = string.Empty;

        if (!string.IsNullOrEmpty(objectDescription))
        {
            textToLog += objectDescription + " ";
        }

        textToLog += $"Object of type {self.GetType()} is null";
        Debug.LogError(textToLog);
    }
}
