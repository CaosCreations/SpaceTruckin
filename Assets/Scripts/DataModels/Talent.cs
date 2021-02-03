using UnityEngine;

public interface ITalent
{
    void ApplyTalents(ScriptableObject target);
}

public abstract class Talent : ScriptableObject, ITalent
{
    protected string TalentName { get; private set; }
    protected string Description { get; private set; }

    public abstract void ApplyTalents(ScriptableObject target);
    

}
