using UnityEngine;
using System.Collections;


[CreateAssetMenu(menuName = "PluggableAI/Actions/Refill")]
public class RefillAction : Action
{
    [Tooltip("Lugar de encher deposito, usado para comparaçao de tags")]
    [SerializeField] GameObject refillPlace;
    private TankFuel tankFuel;

    public override void Act(StateController controller)
    {
        tankFuel = controller.gameObject.GetComponent<TankFuel>();

        if (!tankFuel || !refillPlace) { return; }

        Refill(controller);
    }

    private void Refill(StateController controller)
    {
        RaycastHit hit;
        Debug.DrawRay(controller.eyes.position, controller.eyes.forward.normalized * controller.enemyStats.attackRange, Color.red);

        bool hitFlag = Physics.SphereCast(controller.eyes.position, controller.enemyStats.lookSphereCastRadius, controller.eyes.forward, out hit, controller.enemyStats.attackRange + 1);

        if (hitFlag && hit.collider.CompareTag(refillPlace.tag))
        {
            if (tankFuel.GetCurrentFuel() < tankFuel.GetMaxFuel())
                tankFuel.RefillTank(); ;
        }
    }
}
