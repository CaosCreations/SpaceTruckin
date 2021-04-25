public partial class Licence
{
    public string Name { get => licenceName; set => licenceName = value; }
    public string Description { get => description; }
    public int Tier { get => tier; set => tier = value; }
    public int PointsCost { get => pointsCost; }
    public bool IsOwned { get => saveData.isOwned; set => saveData.isOwned = value; }
    public bool IsUnlocked { get => saveData.isUnlocked; set => saveData.isUnlocked = value; }
    public Licence PrerequisiteLicence { get => prerequisiteLicence; set => prerequisiteLicence = value; }
    public LicenceEffect Effect { get => effect; }
}
