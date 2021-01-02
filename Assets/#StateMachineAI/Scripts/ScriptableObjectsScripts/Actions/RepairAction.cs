using UnityEngine;
using System.Collections;


[CreateAssetMenu(menuName = "PluggableAI/Actions/Repair")]
public class RepairAction : Action
{
    [Tooltip("Lugar de para reparar, usado para comparaçao de tags")]
    [SerializeField] GameObject repairPlace;
    private TankHealth tankHealth;

    public override void Act(StateController controller)
    {
        tankHealth = controller.gameObject.GetComponent<TankHealth>();
        
        if (!tankHealth || !repairPlace) { return; }
        
        Repair(controller);
    }

    private void Repair(StateController controller)
    {
        RaycastHit hit;
        Debug.DrawRay(controller.eyes.position, controller.eyes.forward.normalized * controller.enemyStats.attackRange, Color.red);
        
        bool hitFlag = Physics.SphereCast(controller.eyes.position, controller.enemyStats.lookSphereCastRadius, controller.eyes.forward, out hit, controller.enemyStats.attackRange + 1);

        if (hitFlag && hit.collider.CompareTag(repairPlace.tag))
        {
            if (tankHealth.GetCurrentHealth() < tankHealth.GetMaxHealth())
                tankHealth.StartRepair();
        }
    }
}

