using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditorInternal;
using UnityEngine;
using static UnityEngine.Random;

[Serializable]
public class GameState
{
    [SerializeField] private StateRegister stateRegister;

    private string currentState;

    public void SetCurrentState(string newState)
    {
        if(TryGetState(newState.ToLower().Trim(), out string result) == true)
                currentState = result;
    }

    public bool CheckCurrentState(string state)
    {
        TryGetState(state, out string result);

        return currentState == result;
    }

    public bool CheckCurrentState(IEnumerable<string> states)
    {
        return states.Any(state => TryGetState(state, out string result) && currentState == result);
    }

    private bool TryGetState(string state, out string result)
    {
        state = state.ToLower().Trim();

        foreach (string item in stateRegister.States) 
        {
            if (item == state)
            {
                result = item;
                return true;
            }
        }

        throw new ArgumentException("The state:" + state + ", you tried to get or set doesn't exist in the register. " +
                                     "Please check the spelling");
    }
}
