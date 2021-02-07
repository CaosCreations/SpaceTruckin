using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackjackDealer : BlackjackPlayer
{
    private int forcedToStandThreshold = 17;

    public bool IsForcedToStand { get => handTotal >= forcedToStandThreshold; }
}
