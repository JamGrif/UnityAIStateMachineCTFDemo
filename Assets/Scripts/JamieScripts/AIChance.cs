/*
 *  To give the AI a less predictable state transistion path, some transistions the AI could take aren't gurranted.
 *  The AI will roll some tranistions chances to see if they would take that action.
 *  For example, if the AI is getting attacked and has lower health then healthThreshold they might have a 30% chance to flee.
 *  
 *  The range of numbers rolled is between 0 and 1, so a chance of 0.4 means there is a 40% chance of that action.
 */

public static class AIChance
{
    public const float fleeChance = 0.3f; //If agent is losing fight then this is the % chance of them fleeing

    public const float GetHealthKitChance = 0.3f; //Chance the AI will run towards a health kit when they enter runToEnemyBaseState

    public const float GetPowerUpChance = 0.3f; //Chance the AI will run towards a power up when they enter runToEnemyBaseState

}

public static class AIThreshold
{
    public const float baseDistance = 3.5f; //The distance the AI needs to be to a base in order to be marked as there. 

    public const int healthThreshold = 50; //Number of hit points the AI needs to be below in order to heal or attack an agent

    public const float attackCooldown = 0.15f; //Time to wait before attacking again

    public const int FleeTime = 2; //Seconds the AI will start fleeing before running somewhere random

}