using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;


public class TankBlueMovement : MonoBehaviour
{
    public AudioSource m_MovementAudio;         // Reference to the audio source used to play engine sounds. NB: different to the shooting audio source.
    public AudioClip m_EngineIdling;            // Audio to play when the tank isn't moving.
    public AudioClip m_EngineDriving;           // Audio to play when the tank is moving.
    public float m_PitchRange = 0.2f;           // The amount by which the pitch of the engine noises can vary.
    public Transform wpParent;
    private List<Transform> wayPoints;

    private float m_MovementInputValue;         // The current value of the movement input.
    private float m_TurnInputValue;             // The current value of the turn input.
    private float m_OriginalPitch;              // The pitch of the audio source at the start of the scene.
    private ParticleSystem[] m_particleSystems; // References to all the particles systems used by the Tanks
    private int destPoint = 0;
    private NavMeshAgent nav;

    float timer;
    public float timeBetweenPauses;
    public float pauseFor;
    private int seconds;

    private void OnEnable()
    {
        // We grab all the Particle systems child of that Tank to be able to Stop/Play them on Deactivate/Activate
        // It is needed because we move the Tank when spawning it, and if the Particle System is playing while we do that
        // it "think" it move from (0,0,0) to the spawn point, creating a huge trail of smoke
        m_particleSystems = GetComponentsInChildren<ParticleSystem>();
        for (int i = 0; i < m_particleSystems.Length; ++i)
        {
            m_particleSystems[i].Play();
        }
    }


    private void OnDisable()
    {

        // Stop all particle system so it "reset" it's position to the actual one instead of thinking we moved when spawning
        for (int i = 0; i < m_particleSystems.Length; ++i)
        {
            m_particleSystems[i].Stop();
        }
    }


    private void Start()
    {

        InitiWaypoints();
        // Store the original pitch of the audio source.
        m_OriginalPitch = m_MovementAudio.pitch;

        nav = GetComponent<NavMeshAgent>();
        nav.SetDestination(wayPoints[destPoint].position);
    }


    void InitiWaypoints( )
    {
        wayPoints = new List<Transform>();
        foreach (Transform child in wpParent)
        {
            if (child != wpParent)
            {
                wayPoints.Add(child);
            }
        }
    }


    private void Update()
    {
        EngineAudio();
        timer += Time.deltaTime;
        seconds = (int)(timer % 60);

        if (seconds >= Random.Range(timeBetweenPauses, timeBetweenPauses * 0.5f))
        {
            StartCoroutine(Wait(pauseFor));
        }
        GoToNextWayPoint();

    }


    IEnumerator Wait(float sec)
    {
        // stop the tank for x seconds
        nav.isStopped = true;
        yield return new WaitForSeconds(sec);
        timer = 0;
        nav.isStopped = false;
    }

    private void EngineAudio()
    {
        // If there is no input (the tank is stationary)...
        if (Mathf.Abs(nav.velocity.magnitude) < 0.1f)
        {
            // ... and if the audio source is currently playing the driving clip...
            if (m_MovementAudio.clip == m_EngineDriving)
            {
                // ... change the clip to idling and play it.
                m_MovementAudio.clip = m_EngineIdling;
                m_MovementAudio.pitch = Random.Range(m_OriginalPitch - m_PitchRange, m_OriginalPitch + m_PitchRange);
                m_MovementAudio.Play();
            }
        }
        else
        {
            // Otherwise if the tank is moving and if the idling clip is currently playing...
            if (m_MovementAudio.clip == m_EngineIdling)
            {
                // ... change the clip to driving and play.
                m_MovementAudio.clip = m_EngineDriving;
                m_MovementAudio.pitch = Random.Range(m_OriginalPitch - m_PitchRange, m_OriginalPitch + m_PitchRange);
                m_MovementAudio.Play();
            }
        }
    }


    void GotoNextPoint()
    {
        // Returns if no points have been set up
        if (wayPoints.Count == 0)
            return;

        // Set the agent to go to the currently selected destination.
        nav.destination = wayPoints[destPoint].position;

        // Choose the next point in the array as the destination,
        // cycling to the start if necessary.
        destPoint = (destPoint + 1) % wayPoints.Count;
    }

    void GoToNextWayPoint()
    {
        if (wayPoints != null && !nav.pathPending && nav.remainingDistance < 0.5f)
        {
            if (destPoint >= wayPoints.Count - 1) destPoint = 0;
            else destPoint++;
            nav.SetDestination(wayPoints[destPoint].position);
        }
    }

    //void OnDrawGizmos()
    //{
    //    if (wayPoints == null) return;
    //    Gizmos.color = Color.green;
    //    Gizmos.DrawSphere(wayPoints[destPoint].position, 0.5f);
    //}
}
