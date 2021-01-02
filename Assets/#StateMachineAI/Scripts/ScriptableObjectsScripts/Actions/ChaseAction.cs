using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "PluggableAI/Actions/Chase")]
public class ChaseAction : Action {


    public override void Act(StateController controller){

        Chase(controller);
    }

    private void Chase(StateController controller)
    {
        controller.navMeshAgent.destination = controller.chaseTarget.position;
        controller.navMeshAgent.stoppingDistance = controller.enemyStats.distanceToStopToAttack;
        RaycastHit hit;
        bool hitFlag = Physics.SphereCast(controller.eyes.position, controller.enemyStats.lookSphereCastRadius, controller.eyes.forward, out hit, controller.enemyStats.attackRange);
      
        if (hitFlag)
        {
            if (controller.navMeshAgent.remainingDistance <= controller.navMeshAgent.stoppingDistance && !controller.navMeshAgent.pathPending)            
            {
                controller.navMeshAgent.isStopped = true;
            }
            else
            {
                controller.navMeshAgent.isStopped = false;
            }
        }                

    }

}
