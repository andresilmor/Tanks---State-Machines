using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Decisions/Repair")]
public class RepairDecision : Decision
{
    [Tooltip("Percentagem de vida minima antes de ir reparar")]
    [SerializeField] float minHealthPercentage = 15f;

    public override bool Decide(StateController controller)
    {

        bool needRepair = Check(controller);
        return needRepair;
    }

    private bool Check(StateController controller)
    {
        TankHealth tankHealth = controller.gameObject.GetComponent<TankHealth>();
        if (!tankHealth) { return false; }
        float minHealth = minHealthPercentage * tankHealth.GetMaxHealth() / 100;
        if (tankHealth.GetCurrentHealth() <= minHealth || tankHealth.IsReparing() )
        {
            StopOthersActivities(controller);
            if (!tankHealth.IsReparing())
                tankHealth.UpdateHealthRepair();
            return true;
        }
        
        return false; 
    }

    //Sem este metodo, por exemplo, o Scanner quando foge a meio de recarregar as balas irá continuar a recarregar
    public void StopOthersActivities(StateController controller)
    {
        TankShooting tankShooting = controller.gameObject.GetComponent<TankShooting>();
        if (tankShooting) { tankShooting.TankRechargedAmmoDone(); }

        TankFuel tankFuel = controller.gameObject.GetComponent<TankFuel>();
        if (tankFuel) { tankFuel.StopRefillTank(); };
    }
}