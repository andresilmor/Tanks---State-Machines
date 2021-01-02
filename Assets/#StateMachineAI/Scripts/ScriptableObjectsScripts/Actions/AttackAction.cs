using UnityEngine;
using System.Collections;


[CreateAssetMenu (menuName = "PluggableAI/Actions/Attack")]
public class AttackAction : Action 
{
	
    public override void Act(StateController controller) {

        Attack(controller);
    }

    private void Attack(StateController controller)
    {
        RaycastHit hit;

        Debug.DrawRay(controller.eyes.position, controller.eyes.forward.normalized * controller.enemyStats.attackRange, Color.red);

        // Se o raycast estiver a atingir o Player então significa que pode disparar
        bool hitFlag = Physics.SphereCast(controller.eyes.position, controller.enemyStats.lookSphereCastRadius, controller.eyes.forward, out hit, controller.enemyStats.attackRange +1);
        if (hitFlag && hit.collider.CompareTag("Player"))
        {
            // vai verificar se pode disparar (timer desde o último tiro)
            if (controller.CheckIfCountDownTimerElapsed(controller.enemyStats.attackRate))
            {
                // procede ao ataque 
                controller.tankShooting.Fire(controller.enemyStats.attackForce, controller.enemyStats.attackRate);
            }
        }
    }
}
