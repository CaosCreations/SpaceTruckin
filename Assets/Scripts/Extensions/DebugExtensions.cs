using UnityEngine;

public static class DebugExtensions
{
    public static void LogErrorIfObjectIsNull(this ILogger self, Object objectToLog)
    {
        if (objectToLog == null)
        {
            self.LogError("Object", $"{objectToLog.name} of type {objectToLog.GetType()} is null");
        }
    }
}
