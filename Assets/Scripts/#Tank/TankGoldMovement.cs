using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TankGoldMovement : MonoBehaviour
{
    public AudioSource m_MovementAudio;         // Reference to the audio source used to play engine sounds. NB: different to the shooting audio source.
    public AudioClip m_EngineIdling;            // Audio to play when the tank isn't moving.
    public AudioClip m_EngineDriving;           // Audio to play when the tank is moving.
    public float m_PitchRange = 0.2f;           // The amount by which the pitch of the engine noises can vary.
    public Transform wpParent;
    private Transform _tankCenter;

    private List<Transform> wayPoints;
    private float m_MovementInputValue;         // The current value of the movement input.
    private float m_TurnInputValue;             // The current value of the turn input.
    private float m_OriginalPitch;              // The pitch of the audio source at the start of the scene.
    private ParticleSystem[] m_particleSystems; // References to all the particles systems used by the Tanks
    private int count = 0;
    private NavMeshAgent nav;


    public Transform TankCenter { get { return _tankCenter; } }
    public Vector3 Velocity { get { return nav.velocity; } }



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
        wayPoints = new List<Transform>();
        foreach (Transform child in wpParent)
        {
            if (child != wpParent)
            {
                wayPoints.Add(child);
            }
        }

        foreach (Transform child in transform)
        {
            if (child.name == "TankCenter")
            {
                _tankCenter = child;
            }
        }

        // Store the original pitch of the audio source.
        m_OriginalPitch = m_MovementAudio.pitch;

        nav = GetComponent<NavMeshAgent>();
        nav.SetDestination(wayPoints[count].position);
    }


    private void Update()
    {
        EngineAudio();
        // Adjust the rigidbodies position and orientation in FixedUpdate.
        GoToNextWayPoint();
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

    void GoToNextWayPoint()
    {
        if (wayPoints != null && !nav.pathPending && nav.remainingDistance < 0.5f)
        {
            if (count >= wayPoints.Count - 1) count = 0;
            else count++;
            nav.SetDestination(wayPoints[count].position);
        }
    }

    //void OnDrawGizmos()
    //{
    //    if (wayPoints == null) return;
    //    Gizmos.color = Color.green;
    //    Gizmos.DrawSphere(wayPoints[count].position, 0.5f);
    //}
}
