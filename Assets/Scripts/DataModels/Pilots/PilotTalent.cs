using UnityEngine;

public class PilotTalent : Talent
{
    [Header("Set in Editor")]
    public int flatXpIncrease;
    public float xpMultiplier;
    public int shipDamageDecrease;

    public override void ApplyTalents(ScriptableObject pilot)
    {
        Debug.Log("");
    }

	public double ApplyXpIncrease(double xpGained)
	{
		xpGained += flatXpIncrease;
		xpGained *= xpMultiplier;
        return xpGained;

	}

	public int ApplyShipDamageDecrease(int shipDamage)
	{
		return shipDamage -= shipDamageDecrease;
	}



}
