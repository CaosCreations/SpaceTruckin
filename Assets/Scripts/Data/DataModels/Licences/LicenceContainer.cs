using UnityEngine;

[CreateAssetMenu(fileName = "LicenceContainer", menuName = "ScriptableObjects/Licences/LicenceContainer", order = 1)]
public class LicenceContainer : ScriptableObject
{
    public Licence[] licences;
}
