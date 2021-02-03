using UnityEngine;

/// <summary>
/// We may need a derived class so that we can have MessageOutcome data models as fields.
/// Thank you emails will probably need to contain gifts, like collectible items.
/// Also, it will help Stefano differentiate between Messages and ThankYouMessages.
/// </summary>
[CreateAssetMenu(fileName = "ThankYouMessage", menuName = "ScriptableObjects/Messages/ThankYouMessage", order = 1)]
public class ThankYouMessage : Message 
{

}