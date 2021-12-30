using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class baseSTATE
{

    protected StateMachineMaster statemachine; //Reference to the state machine. Used to change to another state from within a state

    abstract public void EntryAction(AI agent); // Action to do when state is first set to active
    abstract public void ExecuteAction(AI agent); // Action used to test condition on changing to a different state
    abstract public void ExitAction(AI agent); // Action to do during state transistion to a different state
}
