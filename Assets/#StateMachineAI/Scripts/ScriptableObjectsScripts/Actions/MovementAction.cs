using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Actions/MovementAction")]
public class MovementAction : Action
{
    [Tooltip("Colocar PreFab do Waypoint pretendido, para encontrar na cena waypoints com a mesma Tag.")]
    [SerializeField] GameObject destinyWaypoint;
    private Vector3 destinationPosition;

    public override void Act(StateController controller)
    {
        if (!destinyWaypoint) { return; }
        destinationPosition = GameObject.FindGameObjectWithTag(destinyWaypoint.tag).transform.position;
        if(destinationPosition == null) { return;  }
        Move(controller);
       
    }

    private void Move(StateController controller)
    {
        controller.navMeshAgent.destination = destinationPosition;
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

