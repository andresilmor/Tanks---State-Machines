using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Decisions/RefillFuel")]
public class RefillFuelDecision : Decision
{
    [SerializeField] float minAutonomyPercentage = 10f;

    public override bool Decide(StateController controller)
    {

        bool depositEmpty = Check(controller);
        return depositEmpty;
    }

    private bool Check(StateController controller)
    {
        TankFuel tankFuel = controller.gameObject.GetComponent<TankFuel>();
        if (!tankFuel) { return false; }
        float minFuel = minAutonomyPercentage * tankFuel.GetMaxFuel() / 100;
        if (tankFuel.IsRefillingDeposit() || tankFuel.GetCurrentFuel() <= minFuel)
        {
            StopOthersActivities(controller);
            if (!tankFuel.IsRefillingDeposit())
                tankFuel.UpdateRefillQuantity();
            return true;
        }
        return false;
    }

    //Sem este metodo, por exemplo, o Scanner quando foge a meio de recarregar as balas irá continuar a recarregar
    public void StopOthersActivities(StateController controller)
    {
        TankShooting tankShooting = controller.gameObject.GetComponent<TankShooting>();
        if (tankShooting) { tankShooting.TankRechargedAmmoDone(); }

        TankHealth tankHealth = controller.gameObject.GetComponent<TankHealth>();
        if (tankHealth) { tankHealth.StopRepair(); }
    }
}
