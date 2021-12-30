using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachineMaster
{
    private baseSTATE CurrentState; //Current state of agent

    public AI agent; //Owner of the state machine

    //Consturctor to initialise state machine for the ai
    public StateMachineMaster(AI agent)
    {
        this.agent = agent;
        TransitionState(new runToEnemyBaseSTATE(this));
    }

    //AIUpdate gets called from the agents AI script
    public void AIUpdate()
    {
        //Call ExecuteAction of CurrentState
        if (CurrentState != null)
        {
            CurrentState.ExecuteAction(agent);
        }
    }

    public void TransitionState(baseSTATE state)
    {
        //Call ExitAction of CurrentState
        if (CurrentState != null)
        {
            CurrentState.ExitAction(agent);
        }

        //Change to new state
        CurrentState = state;

        //Calls EnterAction of new CurrentState
        if (CurrentState != null)
        {
            CurrentState.EntryAction(agent);
        }
    }
}
