using UnityEngine;
using System.Collections;


[CreateAssetMenu(menuName = "PluggableAI/Actions/Recharge")]
public class RechargeAction : Action
{
    [Tooltip("Lugar de recarregar balas, usado para comparaçao de tags")]
    [SerializeField] GameObject rechargePlace;
    private TankShooting tankShooting;

    public override void Act(StateController controller)
    {
        tankShooting = controller.gameObject.GetComponent<TankShooting>();
        if (!tankShooting || !rechargePlace) { return; }
        Recharge(controller);
    }

    private void Recharge(StateController controller)
    {
        RaycastHit hit;
        Debug.DrawRay(controller.eyes.position, controller.eyes.forward.normalized * controller.enemyStats.attackRange, Color.red);

        bool hitFlag = Physics.SphereCast(controller.eyes.position, controller.enemyStats.lookSphereCastRadius, controller.eyes.forward, out hit, controller.enemyStats.attackRange + 1);
        if (hitFlag && hit.collider.CompareTag(rechargePlace.tag))
        {   
            if(tankShooting.CheckTankLowAmmo())
                tankShooting.RechargeTankAmmo();
        }
    }
}

