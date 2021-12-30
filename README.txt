Demo showcase - https://www.youtube.com/watch?v=ueeuN0IZdbQ

This project is part of a university assignment where I was tasked to extend a simple project by adding AI functionality to the program.

This demo shows a simple capture the flag scenario where two teams consisting of three players need to take the enemy flag, while simultaneously defending their own.

By holding the enemy flag in their own base, they will slowly accumulate points.

The AI uses a Finite State Machine AI system to achieve this, and each agent has 8 states they 
can be in with a bit of randomness sprinkled in to help make things more unpredictable:
attackEnemy
guardFlag
pickupEnemyFlag
pickupFriendlyFlag
runToEnemyBase
runToFriendlyBase
runToHealthKit
runToPowerUp

Note that only scripts within the JamieScripts folder was created by me for the assignment.