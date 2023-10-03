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
        var activator = GetActivatorByName(zoneName);
        activator.Activate();
    }

    public void UnlockZone(string zoneName)
    {
        var activator = GetActivatorByName(zoneName);
        activator.Deactivate();
    }

    private DialoguePositionConstrainerActivator GetActivatorByName(string zoneName)
    {
        var activator = activators.FirstOrDefault(a => a.ZoneName == zoneName);
        if (activator == null)
        {
            throw new Exception("Zone lock - zone name does not exist: " + zoneName);
        }
        return activator;
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
