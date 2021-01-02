using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Decisions/Scan")]
public class ScanDecision : Decision
{
    public override bool Decide(StateController controller)
    {
        bool noEnemyInSight = Scan(controller);
        return noEnemyInSight;
    }

    private bool Scan(StateController controller) {

        // Quando o player sair da sua linha de vista, ele vai parar, vai rodar no eixo dos Ys e procurar pelo player
        //  durante algum tempo e depois volta a patrulhar

        return controller.CheckIfCountDownTimerElapsed(controller.enemyStats.searchDuration);
    }
}
