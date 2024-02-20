public class DemoCharacterCreationNameInputUI : CharacterCreationUI
{
    protected override void Start()
    {
        base.Start();
        okButton.interactable = false;
    }

    protected override void OnValueChanged()
    {
        base.OnValueChanged();
        okButton.interactable = isNameValid;
    }

    // Temporary demo limitation as we do not have confirmation button when setting name 
    protected override void Update()
    {
        return;
    }
}
