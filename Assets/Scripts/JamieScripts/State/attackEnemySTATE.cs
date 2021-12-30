using UnityEngine;

/*
 *      Transition to:
 *      runToEnemyBaseState
 * 
 */

public class attackEnemySTATE : baseSTATE
{
    GameObject enemy;
    string enemyName;
    float attackTime = 0;
    float currentFleeTime = 0;
    bool fleeing = false;
    bool RolledToFlee = false;
    bool HoldingPowerup = false;

    public attackEnemySTATE(StateMachineMaster statemachine)
    {
        this.statemachine = statemachine;
    }

    public override void EntryAction(AI agent)
    {
        Debug.Log(agent.GetAgentData().name + " has entered attackEnemyState");

        enemy = agent.GetAgentSenses().GetEnemiesInView()[0]; //Go for first seen enemy
        enemyName = enemy.name;
        attackTime = 0;
        currentFleeTime = 0;

        fleeing = false;
        RolledToFlee = false;
        HoldingPowerup = false;

        if (agent.GetAgentInventory().GetItem(Names.PowerUp) != null) //Check if holding powerup
        {
            HoldingPowerup = true;
        }
    }

    public override void ExecuteAction(AI agent)
    {
        //Cooldown to ensure enemy only attacks every AI.Threshold.attackCooldown seconds
        if (attackTime > 0)
        {
            attackTime -= Time.deltaTime;
            if (attackTime < 0)
                attackTime = 0;
        }

        if (agent.GetAgentData().CurrentHitPoints < AIThreshold.healthThreshold) //If agent is below healthThreshold, roll to see if they will use health kit or flee
        {
            if (agent.GetAgentInventory().GetItem(Names.HealthKit)) //Check if agent is holding a health kit
            {
                agent.GetAgentActions().UseItem(agent.GetAgentInventory().GetItem(Names.HealthKit));
            }

            if (!RolledToFlee)
            {
                RolledToFlee = true;
                if (Random.value <= AIChance.fleeChance)
                {
                    fleeing = true;
                }
            }
        }

        if (attackTime == 0 && !fleeing) //Ensure the agent is not in a attack cooldown and not fleeing
        {
            if (agent.GetAgentSenses().GetObjectInViewByName(enemyName))//Ensure enemy can still be seen 
            {
                if (agent.GetAgentSenses().IsInAttackRange(enemy)) //Agent is close enough to attack enemy
                {
                    if (HoldingPowerup)
                    {
                        HoldingPowerup = false;
                        agent.GetAgentActions().UseItem(agent.GetAgentInventory().GetItem(Names.PowerUp));
                    }
                    
                    agent.GetAgentActions().AttackEnemy(enemy);
                    attackTime = AIThreshold.attackCooldown;
                }
                else //Agent needs to move to attack range of enemy
                {
                    agent.GetAgentActions().MoveTo(enemy);
                }
            }
            else //Back to default state as enemy is either dead or ran away
            {
                statemachine.TransitionState(new runToEnemyBaseSTATE(statemachine)); 
            }
        }

        if (fleeing)
        {
            if (agent.GetAgentData().CurrentHitPoints > AIThreshold.healthThreshold) //If health is now higher then threshold, then stop fleeing. Healed from health kit
            {
                fleeing = false;
                currentFleeTime = 0;
            }

            currentFleeTime += Time.deltaTime;
            if (currentFleeTime <= AIThreshold.FleeTime) //AI only flees for AIThreshold.Fleetime number of seconds
            {
                if (enemy != null)
                {
                    agent.GetAgentActions().Flee(enemy);
                }
                else //If enemy becomes null, then they died so return back to default stance.
                {
                    statemachine.TransitionState(new runToEnemyBaseSTATE(statemachine));
                }
                
            }
            else //When stopped fleeing, return to default stance
            {
                statemachine.TransitionState(new runToEnemyBaseSTATE(statemachine));
            }
        }
    }

    public override void ExitAction(AI agent)
    {
        Debug.Log(agent.GetAgentData().name + " has exitted attackEnemyState");

    }
}
