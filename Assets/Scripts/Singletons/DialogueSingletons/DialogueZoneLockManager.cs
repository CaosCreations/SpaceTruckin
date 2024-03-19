using PixelCrushers.DialogueSystem;
using System;
using System.Linq;
using UnityEngine;

public class DialogueZoneLockManager : MonoBehaviour, ILuaFunctionRegistrar
{
    public static DialogueZoneLockManager Instance { get; private set; }

    private static DialoguePositionConstrainerActivator[] activators;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        activators = FindObjectsOfType<DialoguePositionConstrainerActivator>();
    }

    public void LockZone(string zoneName)
    {
        var activators = GetActivatorsByName(zoneName);
        Array.ForEach(activators, a => a.Activate());
    }

    public void UnlockZone(string zoneName)
    {
        var activators = GetActivatorsByName(zoneName);
        Array.ForEach(activators, a => a.Deactivate());
    }

    private DialoguePositionConstrainerActivator[] GetActivatorsByName(string zoneName)
    {
        // There can be multiple instances per zone 
        var activatorsByName = activators.Where(a => a.ZoneName == zoneName);
        if (!activatorsByName.Any())
        {
            Debug.LogError("Zone lock - zone name does not exist: " + zoneName);
        }
        return activatorsByName.ToArray();
    }

    public void RegisterLuaFunctions()
    {
        Lua.RegisterFunction(
            DialogueConstants.LockZoneFunctionName,
            this,
            SymbolExtensions.GetMethodInfo(() => LockZone(string.Empty)));

        Lua.RegisterFunction(
            DialogueConstants.UnlockZoneFunctionName,
            this,
            SymbolExtensions.GetMethodInfo(() => UnlockZone(string.Empty)));
    }

    public void UnregisterLuaFunctions()
    {
        Lua.UnregisterFunction(DialogueConstants.LockZoneFunctionName);
    }
}
