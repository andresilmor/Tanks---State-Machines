using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "PluggableAI/Actions/PatrolAction")]
public class PatrolAction : Action {


    public override void Act(StateController controller) {

        Patrol(controller);
    }
	

    private void Patrol(StateController controller) {
        controller.navMeshAgent.stoppingDistance = controller.enemyStats.distanceToStopToWayPoint;
        controller.navMeshAgent.destination = controller.wayPointList[controller.nextWayPoint].position;
        controller.navMeshAgent.isStopped = false;

        // verifica se já chegamos ao destino, isso acontece quando a distância restante for menor 
        // que a distância para ele parar e quando já não restar caminho para fazer
        if(controller.navMeshAgent.remainingDistance <= controller.navMeshAgent.stoppingDistance && !controller.navMeshAgent.pathPending) {

            // garante que não tentamos aceder a um waipoint fora da lista, caso isso aconteça passamos para o 1º waipoint
            controller.nextWayPoint = (controller.nextWayPoint + 1) % controller.wayPointList.Count;
        }
    }	
}
