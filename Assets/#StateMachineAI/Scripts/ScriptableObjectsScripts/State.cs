using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu (menuName = "PluggableAI/State")]
public class State : ScriptableObject {

    public string stateName;
    public Action[] actions;
    public Color sceneGizmoColor = Color.grey;
    public Transition[] transitions;


    public void UpdateState(StateController controller) {

        // como esta classe é um Asset é comporta-se como um material (se alterarmos a cor alteramos para todos os renders que 
        // utilizam esse material. Aqui passa-se a mesma coisa estes Assets (ScriptableObject) vão ser partilhados pelos  
        // vários tanques e cada um dos tanques vai ter valores diferentes de cada estado, ação, decisão, e como tal 
        // sempre que um atualizamos o estado de um tanque, temos que passar esse tanque ao asset
        // é a referencia a cada um dos controllers que vai conter toda a informação dele, a informação de cada instância de um objeto 
        // não pode ficar guardada no asset.
        DoActions(controller);
        CheckTransitions(controller);
    }

    // vai fazer as ações de cada Estado
    private void DoActions(StateController controller) {

        for (int i = 0; i < actions.Length; i++)
        {            
            actions[i].Act(controller);
        }
    }

    private void CheckTransitions(StateController controller) {

        for (int i = 0; i < transitions.Length; i++)
        {
            bool decisionSucceeded = transitions[i].decision.Decide(controller);

            if(decisionSucceeded) {

                controller.TransitionToState((transitions[i].trueState));
            } else {

                controller.TransitionToState(transitions[i].falseState);
            }
        }
    }


}
