using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Decisions/Arrived")]
public class ArrivedDecision : Decision
{

    public override bool Decide(StateController controller)
    {

        bool hasArrived = Arrived(controller);
        return hasArrived;
    }

    private bool Arrived(StateController controller)
    {
        if (controller.navMeshAgent.isStopped)
            return true;
        return false;
    }
}
