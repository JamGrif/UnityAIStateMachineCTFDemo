using UnityEngine;

/*
 *      Transition to:
 *      attackEnemyState
 *      pickupEnemyFlagState
 *      pickupFriendlyFlagState
 *      runToPowerupState
 *      runToHealthKitState
 *      guardFlagState
 */

public sealed class runToEnemyBaseSTATE : baseSTATE
{
    GameObject healthkit;
    GameObject powerup;

    //These variables ensure that a roll to get the health or powerup collectables is only done once every time this state is transistioned to
    bool RollToGetHealth = false;
    bool RollToGetPowerup = false;
    bool StoppedRunningToEnemyBase = false;

    public runToEnemyBaseSTATE(StateMachineMaster statemachine)
    {
        this.statemachine = statemachine;
    }

    public override void EntryAction(AI agent)
    {
        Debug.Log(agent.GetAgentData().name + " has entered runToEnemyBaseState");

        if (GameObject.Find("Health Kit") != null)
            healthkit = GameObject.Find("Health Kit");

        if (GameObject.Find("Power Up") != null)
            powerup = GameObject.Find("Power Up");

        agent.GetAgentActions().MoveTo(agent.GetAgentData().EnemyBase.gameObject); //Run to enemy base

        RollToGetHealth = false;
        RollToGetPowerup = false;
        StoppedRunningToEnemyBase = false;
    }

    public override void ExecuteAction(AI agent)
    {
        if (!StoppedRunningToEnemyBase)
        {
            agent.GetAgentActions().MoveTo(agent.GetAgentData().EnemyBase.gameObject); //Run to enemy base
        }

        //guardFlagState
        if (agent.GetAgentData().FriendlyBase.GetComponent<SetScore>().GetEnemyFlagInBase()) //If enemy flag is in base
        {
            if (agent.GetAgentData().FriendlyBase.GetComponent<SetScore>().GetFriendlyFlagInBase()) //and if own flag is in base
            {
                statemachine.TransitionState(new guardFlagState(statemachine));
            }
        }

        // pickupFriendlyFlagState
        if (!agent.GetAgentData().FriendlyBase.GetComponent<SetScore>().GetFriendlyFlagInBase()) //Check if friendly flag is in own base or not
        {
            if (agent.GetAgentSenses().GetObjectInViewByName(agent.GetAgentData().FriendlyFlagName) == true)
            {
                statemachine.TransitionState(new pickupFriendlyFlagSTATE(statemachine));
            }
        }

        // pickupEnemyFlagState
        if (!agent.GetAgentData().FriendlyBase.GetComponent<SetScore>().GetEnemyFlagInBase()) //Only check for enemy flag if not in friendly base
        {
            if (agent.GetAgentSenses().GetObjectInViewByName(agent.GetAgentData().EnemyFlagName) == true) 
            {
                statemachine.TransitionState(new pickupEnemyFlagSTATE(statemachine));
            }
        }

        // attackEnemyState
        if (agent.GetAgentSenses().GetEnemiesInView().Count > 0) //Check for nearby enemies
        {
            statemachine.TransitionState(new attackEnemySTATE(statemachine));
            
        }

        //heal
        if (agent.GetAgentData().CurrentHitPoints < AIThreshold.healthThreshold) //If agent is below healthThreshold, they should see if they are holding health kit
        {
            if (agent.GetAgentInventory().GetItem(Names.HealthKit)) //Check if agent is holding a health kit
            {
                agent.GetAgentActions().UseItem(agent.GetAgentInventory().GetItem(Names.HealthKit));
            }
        }

        // runToHealthKitState
        if (!agent.GetAgentInventory().HasItem(Names.HealthKit)) //If not holding health kit already
        {
            if (agent.GetAgentSenses().GetObjectInViewByName(Names.HealthKit)) //If health kit in view, run towards it
            {
                statemachine.TransitionState(new runToHealthKitSTATE(statemachine));
            }

            if (GameObject.Find("Health Kit") != null)
                healthkit = GameObject.Find("Health Kit");
            else
            {
                healthkit = null;
                StoppedRunningToEnemyBase = false;
            }
                
            //Roll to see if they should run towards a health kit thats not in view
            if (!RollToGetHealth)
            {
                RollToGetHealth = true;
                if (Random.value <= AIChance.GetHealthKitChance)
                {
                    StoppedRunningToEnemyBase = true;
                    agent.GetAgentActions().MoveTo(healthkit); //--- Have to have this instead of state transition as runToHealthKit works on direct line of sight to health kit
                    //statemachine.TransitionState(new runToHealthKitSTATE(statemachine));
                }
            }
        }

        // runToPowerupState
        if (!agent.GetAgentInventory().HasItem(Names.PowerUp)) //If not holding health kit already
        {
            if (agent.GetAgentSenses().GetObjectInViewByName(Names.PowerUp)) //If health kit in view
            {
                statemachine.TransitionState(new runToPowerUpSTATE(statemachine));
            }

            if (GameObject.Find("Power Up") != null)
                powerup = GameObject.Find("Power Up");
            else
            {
                powerup = null;
                StoppedRunningToEnemyBase = false;
            }

            //Roll to see if they should run towards a health kit thats not in view
            if (!RollToGetPowerup)
            {
                RollToGetPowerup = true;
                if (Random.value <= AIChance.GetPowerUpChance)
                {
                    StoppedRunningToEnemyBase = true;
                    agent.GetAgentActions().MoveTo(powerup); //--- Have to have this instead of state transition as runToPowerUp works on direct line of sight to power up
                    //statemachine.TransitionState(new runToPowerUpSTATE(statemachine));
                }
            }
        }
    }

    public override void ExitAction(AI agent)
    {
        Debug.Log(agent.GetAgentData().name + " has exitted runToEnemyBaseState");

    }
}
