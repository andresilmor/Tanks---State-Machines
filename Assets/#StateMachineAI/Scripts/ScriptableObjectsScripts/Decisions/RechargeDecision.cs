using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Decisions/Recharge")]
public class RechargeDecision : Decision
{

    public override bool Decide(StateController controller)
    {

        bool needRecharge = Check(controller);
        return needRecharge;
    }

    private bool Check(StateController controller)
    { 
        TankShooting tankShooting = controller.gameObject.GetComponent<TankShooting>();
        if (!tankShooting) { return false; }
        StopOthersActivities(controller);
        if (tankShooting.CheckTankLowAmmo() || tankShooting.IsRechargingAmmo())
            return true;
        return false;
    }

    //Sem este metodo, por exemplo, o Scanner quando foge a meio de recarregar as balas irá continuar a recarregar
    public void StopOthersActivities(StateController controller)
    {
        TankHealth tankHealth = controller.gameObject.GetComponent<TankHealth>();
        if (tankHealth) { tankHealth.StopRepair(); }

        TankFuel tankFuel = controller.gameObject.GetComponent<TankFuel>();
        if (tankFuel) { tankFuel.StopRefillTank(); }
    }
}

