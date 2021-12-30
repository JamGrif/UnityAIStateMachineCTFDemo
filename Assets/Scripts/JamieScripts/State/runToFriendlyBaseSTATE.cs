using UnityEngine;

/*
 *      Transition to:
 *      runToEnemyBaseState
 * 
 * 
 */

public class runToFriendlyBaseSTATE : baseSTATE
{
    public runToFriendlyBaseSTATE(StateMachineMaster statemachine)
    {
        this.statemachine = statemachine;
    }

    public override void EntryAction(AI agent)
    {
        Debug.Log(agent.GetAgentData().name + " has entered runToFriendlyBaseState");

        agent.GetAgentActions().MoveTo(agent.GetAgentData().FriendlyBase.gameObject);
    }

    public override void ExecuteAction(AI agent)
    {
        if (agent.GetAgentData().CurrentHitPoints < AIThreshold.healthThreshold) //If agent is below healthThreshold, they should see if they are holding health kit
        {
            if (agent.GetAgentInventory().GetItem(Names.HealthKit)) //Check if agent is holding a health kit
            {
                agent.GetAgentActions().UseItem(agent.GetAgentInventory().GetItem(Names.HealthKit));
            }
        }

        if (agent.GetAgentInventory().HasItem(agent.GetAgentData().EnemyFlagName)) //Agent is holding enemy flag
        {
            if (Vector3.Distance(agent.transform.position, agent.GetAgentData().FriendlyBase.transform.position) <= AIThreshold.baseDistance) //Agent within the base area
            {
                agent.GetAgentActions().DropItem(agent.GetAgentInventory().GetItem(agent.GetAgentData().EnemyFlagName));
            }
        }
        else if (agent.GetAgentInventory().HasItem(agent.GetAgentData().FriendlyFlagName))
        {
            if (Vector3.Distance(agent.transform.position, agent.GetAgentData().FriendlyBase.transform.position) <= AIThreshold.baseDistance) //Agent within the base area
            {
                agent.GetAgentActions().DropItem(agent.GetAgentInventory().GetItem(agent.GetAgentData().FriendlyFlagName));
            }
        }
        else
        {
            statemachine.TransitionState(new runToEnemyBaseSTATE(statemachine));
        }

    }

    public override void ExitAction(AI agent)
    {
        Debug.Log(agent.GetAgentData().name + " has exitted runToFriendlyBaseState");
        
    }
}
