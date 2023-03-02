using System;
using System.Collections.Generic;

[Serializable]
public class ArchivedMissionOutcomeContainer
{
    public List<ArchivedMoneyOutcome> MoneyOutcomes = new();
    public List<ArchivedPilotXpOutcome> ArchivedPilotXpOutcomes = new();
    public List<ArchivedShipDamageOutcome> ArchivedShipDamageOutcomes = new();
}