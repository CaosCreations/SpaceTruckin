using UnityEngine;

public class MissionConstants : MonoBehaviour
{
    // AvailableJobsContainer dimensions  
    public static readonly Vector2 AvailableJobsContainerAnchorMin = Vector2.zero;
    public static readonly Vector2 AvailableJobsContainerAnchorMax = new Vector2(0.5f, 1f);
    public static readonly Vector2 AvailableJobsContainerLocalPosition = new Vector2(-0.5f, -0.5f);
    public static readonly Vector2 AvailableJobsContainerLocalScale = new Vector2(0.75f, 0.75f);
    public static readonly Vector2 AvailableJobsGridCellSize = new Vector2(128f, 128f);
    public static readonly Vector2 AvailableJobsGridSpacing = new Vector2(64f, 64f);
    public static readonly Vector2 AvailableJobsContainerLeftPadding = new Vector2(64f, 64f);

    // ScheduleContainer dimensions 
    public const int ScheduleSlots = 14;
    public static readonly Vector2 ScheduleContainerAnchorMin = new Vector2(0.5f, 0f);
    public static readonly Vector2 ScheduleContainerAnchorMax = Vector2.one;
    public static readonly Vector2 ScheduleContainerLocalScale = new Vector2(0.75f, 0.75f);
    public const int ScheduleContainerSpacing = 32;
    public const int ScheduleContainerLeftPadding = 32;

    // Common dimensions 
    public const int GridTopPadding = 16;
    public const int GridLeftPadding = 16;
    public static readonly Vector2 GridCellSize = new Vector2(80f, 80f);
    public static readonly Vector2 GridSpacing = new Vector2(32f, 32f);

    // Game object names 
    public const string AvailableJobsContainerName = "AvailableJobsContainer";
    public const string ScheduleContainerName = "ScheduleContainer";
    public const string ScheduleSlotName = "ScheduleSlot";

    // Sprite paths
    public const string JobSystemFolderPath = "Sprites/JobSystem/";
    public const string ContainerBackgroundSpritePath = JobSystemFolderPath + "ScheduleBackground";
    public const string ScheduleSlotSpritePath = JobSystemFolderPath + "ScheduleSlot";
    public const string JobSpritePath = JobSystemFolderPath + "Briefcase";

    // Drag and drop parameters 
    public const float DragAlpha = 0.5f;
    public const float DropAlpha = 1f;

    // Tags 
    public const string MissionsListRaycastTag = "MissionsListRaycast";
}
