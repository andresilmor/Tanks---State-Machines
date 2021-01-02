using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "PluggableAI/Actions/Scan")]
public class ScanAction : Action {

    public override void Act(StateController controller) {

        Scan(controller);
    }

    void Scan(StateController controller) {

        controller.navMeshAgent.isStopped = true;
        controller.transform.Rotate(0, controller.enemyStats.searchingTurnSpeed * Time.deltaTime, 0);
    }

}
