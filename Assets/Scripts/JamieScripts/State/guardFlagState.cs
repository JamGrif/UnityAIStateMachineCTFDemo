using UnityEngine;

/*
 *      Transition to:
 *      attackEnemySTATE
 *      runToEnemyBaseState
 * 
 */

public class guardFlagState : baseSTATE
{
    //private bool InBase;
    public guardFlagState(StateMachineMaster statemachine)
    {
        this.statemachine = statemachine;
    }

    public override void EntryAction(AI agent)
    {
        Debug.Log(agent.GetAgentData().name + " has entered guardFlagState");

    }

    public override void ExecuteAction(AI agent)
    {

        //If in friendly base stand still
        if (Vector3.Distance(agent.transform.position, agent.GetAgentData().FriendlyBase.transform.position) <= AIThreshold.baseDistance)
        {
            agent.GetAgentActions().MoveTo(agent.transform.position);
        }
        //else run to friendly base
        else
        {
            agent.GetAgentActions().MoveTo(agent.GetAgentData().FriendlyBase.gameObject);
        }

        //Check nearby for enemies
        if (agent.GetAgentSenses().GetEnemiesInView().Count > 0) 
        {
            statemachine.TransitionState(new attackEnemySTATE(statemachine));

        }

        //If flag is no longer in base, then return to default state
        if (!agent.GetAgentData().FriendlyBase.GetComponent<SetScore>().GetEnemyFlagInBase())
        {
            statemachine.TransitionState(new runToEnemyBaseSTATE(statemachine));
        }
    }

    public override void ExitAction(AI agent)
    {
        Debug.Log(agent.GetAgentData().name + " has exitted guardFlagState");

    }
}
