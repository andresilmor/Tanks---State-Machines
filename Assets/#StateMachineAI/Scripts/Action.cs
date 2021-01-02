using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Action : ScriptableObject {

    // abstract indica que esta classe tem a intenção de apenas ser a classe base de outas classes
    public abstract void Act(StateController controller);

}
