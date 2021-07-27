using System;

/// <summary>
/// <para> Multiply the rewards from MissionOutcomes by the relevant value.</para>
/// <para> They're always greater than 1.</para>
/// </summary>
[Serializable]
public struct BonusExponents
{
    public float MoneyExponent;
    public float XpExponent;
}