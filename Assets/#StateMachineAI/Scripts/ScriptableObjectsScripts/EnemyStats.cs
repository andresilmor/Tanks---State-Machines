using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "PluggableAI/EnemyStats")]
public class EnemyStats : ScriptableObject {


    public float moveSpeed = 1;
	public float lookRange = 40f;
	public float lookSphereCastRadius = 1f;
    public float waypointRange = 1;

    public float attackRange = 6;
	public float attackRate = 1f;
	public float attackForce = 15f;
	public int attackDamage = 50;

    public float distanceToStopToAttack = 6;
    public float distanceToStopToWayPoint = 1;



    public float searchDuration = 1f;
	public float searchingTurnSpeed = 120f;

    //jpaulo
    public int m_MaxAmmo = 10;
    public float maxFuellCapacity = 100;
    public float minFuellCapacity = 10;
}