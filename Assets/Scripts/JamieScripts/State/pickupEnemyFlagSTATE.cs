using UnityEngine;

/*
 *      Transition to:
 *      runToFriendlyBaseState
 *      runToEnemyBaseSTATE
 * 
 */

public class pickupEnemyFlagSTATE : baseSTATE
{
    GameObject enemyflag;
    public pickupEnemyFlagSTATE(StateMachineMaster statemachine)
    {
        this.statemachine = statemachine;
    }

    public override void EntryAction(AI agent)
    {
        Debug.Log(agent.GetAgentData().name + " has entered pickupEnemyFlagState");

        enemyflag = agent.GetAgentSenses().GetObjectInViewByName(agent.GetAgentData().EnemyFlagName);
    }

    public override void ExecuteAction(AI agent)
    {
        if (agent.GetAgentData().CurrentHitPoints < AIThreshold.healthThreshold) //If agent is below healthThreshold, roll to see if they will use health kit or flee
        {
            if (agent.GetAgentInventory().GetItem(Names.HealthKit) != null) //Check if agent is holding a health kit
            {
                agent.GetAgentActions().UseItem(agent.GetAgentInventory().GetItem(Names.HealthKit));
            }
        }

        if (agent.GetAgentSenses().GetObjectInViewByName(agent.GetAgentData().EnemyFlagName)) //Check that flag is still there while running to it
        {
            agent.GetAgentActions().MoveTo(enemyflag); //Move towards flag
            agent.GetAgentActions().CollectItem(enemyflag); //Pickup the flag
            if (agent.GetAgentInventory().HasItem(agent.GetAgentData().EnemyFlagName)) //If agent picked up the flag, then run back to friendly base
            {
                statemachine.TransitionState(new runToFriendlyBaseSTATE(statemachine));
            }
        }
        else //Otherwise just go back to default state
        {
            statemachine.TransitionState(new runToEnemyBaseSTATE(statemachine));
        }
    }

    public override void ExitAction(AI agent)
    {
        Debug.Log(agent.GetAgentData().name + " has exitted pickupEnemyFlagState");

    }
}
