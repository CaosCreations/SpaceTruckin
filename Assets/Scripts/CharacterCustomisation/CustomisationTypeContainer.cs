using UnityEngine;

[CreateAssetMenu(fileName = "CustomisationTypeContainer", menuName = "ScriptableObjects/CustomisationOptionContainer", order = 1)]
public class CustomisationTypeContainer : ScriptableObject, IScriptableObjectContainer<CustomisationType>
{
    [field: SerializeField]
    public CustomisationType[] Elements { get; set; }
}
