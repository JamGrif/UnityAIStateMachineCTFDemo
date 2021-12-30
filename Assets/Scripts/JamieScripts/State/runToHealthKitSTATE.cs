using UnityEngine;

/*
 *      Transition to:
 *      runToEnemyBaseState
 *      
 * 
 */

public class runToHealthKitSTATE : baseSTATE
{
    GameObject healthkit;

    public runToHealthKitSTATE(StateMachineMaster statemachine)
    {
        this.statemachine = statemachine;
    }

    public override void EntryAction(AI agent)
    {
        Debug.Log(agent.GetAgentData().name + " has entered runToHealthKitState");

    }

    public override void ExecuteAction(AI agent)
    {
        healthkit = agent.GetAgentSenses().GetObjectInViewByName(Names.HealthKit); 
        
        if (healthkit == null) //If health kit is no longer visible, go back to default state
        {
            statemachine.TransitionState(new runToEnemyBaseSTATE(statemachine));
        }
        else //Otherwise move towards and pickup health kit
        {
            if (healthkit != null)
            {
                agent.GetAgentActions().MoveTo(healthkit); //Moves the agent towards the pickup
                agent.GetAgentActions().CollectItem(healthkit); //Attempts to collect the pickup if it is in range
            }
        }

        if (agent.GetAgentInventory().HasItem(Names.HealthKit)) //If holding health kit, return to default state
        {
            statemachine.TransitionState(new runToEnemyBaseSTATE(statemachine));
        }
        
    }

    public override void ExitAction(AI agent)
    {
        Debug.Log(agent.GetAgentData().name + " has exitted runToHealthKitState");

    }
}
