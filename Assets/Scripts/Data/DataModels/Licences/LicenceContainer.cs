using UnityEngine;

[CreateAssetMenu(fileName = "LicenceContainer", menuName = "ScriptableObjects/Licences/LicenceContainer", order = 1)]
public class LicenceContainer : ScriptableObject, IScriptableObjectContainer<Licence>
{
    [field: SerializeField]
    public Licence[] Elements { get; set; }
}
