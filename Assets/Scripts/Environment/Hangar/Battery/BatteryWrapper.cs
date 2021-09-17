using System;
using UnityEngine;

[RequireComponent(typeof(BatteryCharging), typeof(BatteryInteractable))]
public class BatteryWrapper : MonoBehaviour
{
    public BatteryInteractable BatteryInteractable;
    public BatteryCharging BatteryCharging;

    #region Persistence
    public const string FolderName = "HangarSaveData";
    public const string FileName = "BatterySaveData";
    public static string FilePath
    {
        get => DataUtils.GetSaveFilePath(FolderName, FileName);
    }
    #endregion
}

[Serializable]
public struct BatterySaveData
{
    public bool IsCharged;
    public Vector3 PositionInHangar;
}
