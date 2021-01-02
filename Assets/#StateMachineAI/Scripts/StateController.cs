using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Complete;

public class StateController : MonoBehaviour {

    public State currentState;
    // referência às estatísticas do inimigo
	public EnemyStats enemyStats;
    // transform que fica à frente do tank, com o objetivo de ser usado nos raycasts
	public Transform eyes;
    // estado atual
    public State remainState;

	[HideInInspector] public NavMeshAgent navMeshAgent;
	[HideInInspector] public TankShooting tankShooting;


	[HideInInspector] public List<Transform> wayPointList;
    [HideInInspector] public int nextWayPoint;
    [HideInInspector] public Transform chaseTarget;
    // tempo mínimo para ficar no estado antes de disparar outra vez
    [HideInInspector] public float stateTimeElapsed;

	private bool aiActive;

	void Awake () 
	{
        tankShooting = GetComponent<TankShooting> ();
		navMeshAgent = GetComponent<NavMeshAgent> ();
    }

    private void Start()
    {
    }

    // configura a AI no statecontroler
    // vai buscar os waypoints específicos de um tank e vê se AI está ativa para esse stateController
    public void SetupAI(bool aiActivationFromTankManager, List<Transform> wayPointsFromTankManager)
	{
		wayPointList = wayPointsFromTankManager;
		aiActive = aiActivationFromTankManager;
		if (aiActive) 
		{
			navMeshAgent.enabled = true;
		} else 
		{
			navMeshAgent.enabled = false;
		}
	}

    private void Update()
    {
        if (!aiActive) 
            return;

        currentState.UpdateState(this);
    }

    private void OnDrawGizmos()
    {
        if(currentState != null && eyes != null) {

            Gizmos.color = currentState.sceneGizmoColor;
            Gizmos.DrawWireSphere(eyes.position,enemyStats.lookSphereCastRadius);
        }
    }

    public void TransitionToState(State nextState) {

        if(nextState != remainState) {

            currentState = nextState;
            OnExitState();
        }
    }

    // implementa um timer de tempo timer
    // retorna true se já passou o tempo definido em timer
    public bool CheckIfCountDownTimerElapsed(float timer) {

        stateTimeElapsed += Time.deltaTime;
        return (stateTimeElapsed >= timer);     
    }


    // faz o reset do timer quando muda de estado
    private void OnExitState() {

        Debug.Log(" Saí do estado: " + currentState.stateName) ;
        stateTimeElapsed = 0;
    }
}