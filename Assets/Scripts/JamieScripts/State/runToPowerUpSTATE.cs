using UnityEngine;

/*
 *      Transition to:
 *      runToEnemyBaseState
 * 
 * 
 */

public class runToPowerUpSTATE : baseSTATE
{
    GameObject powerup;

    public runToPowerUpSTATE(StateMachineMaster statemachine)
    {
        this.statemachine = statemachine;
    }

    public override void EntryAction(AI agent)
    {
        Debug.Log(agent.GetAgentData().name + " has entered runToPowerUpState");

    }

    public override void ExecuteAction(AI agent)
    {
        powerup = agent.GetAgentSenses().GetObjectInViewByName(Names.PowerUp);

        if (powerup == null) //If power up is no longer visible, go back to default state
        {
            statemachine.TransitionState(new runToEnemyBaseSTATE(statemachine));
        }
        else //Otherwise move towards and pickup power up
        {
            if (powerup != null)
            {
                agent.GetAgentActions().MoveTo(powerup); //Moves the agent towards the pickup
                agent.GetAgentActions().CollectItem(powerup); //Attempts to collect the pickup if it is in range
            }
            
        }

        if (agent.GetAgentInventory().HasItem(Names.PowerUp)) //If holding power up, return to default state
        {
            statemachine.TransitionState(new runToEnemyBaseSTATE(statemachine));
        }
    }

    public override void ExitAction(AI agent)
    {
        Debug.Log(agent.GetAgentData().name + " has exitted runToPowerUpState");

    }
}
