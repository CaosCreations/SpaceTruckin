using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackjackDealer : BlackjackPlayer
{
    private int forcedToStandThreshold = 17;

    //private bool IsForcedToStand { get => handTotal >= forcedToStandThreshold; }

    public bool IsStanding { get => handTotal >= forcedToStandThreshold && !IsBust; }

    public override void CheckHandTotal()
    {
        //if (handTotal >= )

        //if (IsForcedToStand)
        //{
        //    isStanding = true;
        //}
    }
}
