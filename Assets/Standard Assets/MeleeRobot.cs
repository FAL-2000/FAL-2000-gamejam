using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using UnityStandardAssets.CrossPlatformInput;

public class MeleeRobot : RobotMovement {
    public enum AIState { Patrol, Wait, Attack, None };
    public GameObject[] patrolPath;
    public float fov;
    public float patrolWaitTime;
    public float lookAroundTime;
    public float punchDistance;
    public float lookAroundAngle;
    public AudioClip alert;
    public AudioClip DeathSound;

    private Vector3 m_lastVelocity = Vector3.zero;
    private AIState m_aiState = AIState.Patrol;
    private Vector3 m_currentDest;
    private int m_currentPatrolPoint;
    private float m_waitTime;
    private float m_lookAround;
    public override void Move()
    {
        GameObject plr = GameObject.FindGameObjectWithTag("Player");
        NavMeshAgent nav = GetComponent<NavMeshAgent>();
        m_lastVelocity = nav.velocity;
        switch (m_aiState)
        {
            case AIState.Attack:
                if (CanSee(plr))
                {
                    m_currentDest = plr.transform.position;
                    transform.forward = plr.transform.position - transform.position;
                }
                if (CanSee(plr) && CloseToTarget(plr.transform.position, punchDistance))
                {
                    plr.GetComponent<TimeManipulatingPlayer>().Die();
                }
                if (!CanSee(plr) && CloseToTarget(m_currentDest, 1))
                {
                    m_aiState = AIState.Patrol;
                }
                break;
            case AIState.Patrol:
                m_currentDest = NextPatrolPoint().transform.position;
                if (CloseToTarget(NextPatrolPoint().transform.position, 1))
                {
                    m_lookAround = 0;
                    m_waitTime = patrolWaitTime;
                    m_aiState = AIState.Wait;
                }
                if (CanSee(plr))
                {
                    m_aiState = AIState.Attack;
                    AudioSource.PlayClipAtPoint(alert, plr.transform.position);
                }
                break;
            case AIState.Wait:
                m_currentDest = NextPatrolPoint().transform.position;
                m_waitTime -= Time.fixedDeltaTime;
                m_lookAround += 2 * Mathf.PI * Time.fixedDeltaTime / lookAroundTime;
                transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, NextPatrolPoint().transform.rotation.eulerAngles.y + Mathf.Sin(m_lookAround) * lookAroundAngle / 2, transform.rotation.eulerAngles.z);
                if (m_waitTime <= 0)
                {
                    m_currentPatrolPoint = (m_currentPatrolPoint + 1) % patrolPath.Length;
                    m_aiState = AIState.Patrol;
                }
                if (CanSee(plr))
                {
                    m_aiState = AIState.Attack;
                    AudioSource.PlayClipAtPoint(alert, plr.transform.position);
                }
                break;
            default:
                break;
        }

        // Serialilze custom properties to string (Pay attention how to serialize Vector3)
        List<string> state = new List<string>();
        state.Add(m_aiState.ToString());
        state.Add(SerializeVector(m_currentDest));
        state.Add(m_currentPatrolPoint.ToString());
        state.Add(m_waitTime.ToString());
        state.Add(m_lookAround.ToString());
        GetComponent<TimeManipulated>().SetRobotState(state);
    }

    private bool CloseToTarget(Vector3 target, float tol)
    {
        return (transform.position - target).sqrMagnitude < tol * tol;
    }

    private bool CanSee(GameObject target)
    {
        RaycastHit hit;
        Physics.Raycast(transform.position, target.transform.position - transform.position, out hit);
        if (hit.transform == null)
            return false;

        Vector3 targetDir = (target.transform.position - transform.position).normalized;
        float halfFov = Mathf.Deg2Rad * fov / 2;
        return Vector3.Dot(targetDir, transform.forward) > Mathf.Cos(halfFov) && hit.transform.gameObject == target;
    }

    private GameObject NextPatrolPoint()
    {
        return patrolPath[m_currentPatrolPoint];
    }

    public override void SetRobotState(List<string> robotState)
    {
        m_aiState = (AIState)Enum.Parse(typeof(AIState), robotState[0]);
        m_currentDest = DeserializeVector(robotState[1]);
        m_currentPatrolPoint = int.Parse(robotState[2]);
        m_waitTime = float.Parse(robotState[3]);
        m_lookAround = float.Parse(robotState[4]);
    }

    public override void TimeStateChange(TimeState old, TimeState nu)
    {
        if (nu == TimeState.Normal)
        {
            GetComponent<NavMeshAgent>().Resume();
        }
        if (old != TimeState.Normal && nu == TimeState.Normal)
        {
            GetComponent<NavMeshAgent>().velocity = m_lastVelocity;
        }
        if (nu != TimeState.Normal)
        {
            GetComponent<NavMeshAgent>().velocity = Vector3.zero;
            GetComponent<NavMeshAgent>().Stop();
        }
    }

    // Use this for initialization
    void Start () {
        if (patrolPath.Length <= 0)
        {
            patrolPath = new GameObject[1];
            patrolPath[0] = new GameObject();
            patrolPath[0].transform.position = transform.position;
            patrolPath[0].transform.rotation = transform.rotation;
        }
        m_currentPatrolPoint = 0;
	}
	
	// Update is called once per frame
	void Update () {
        GetComponent<NavMeshAgent>().SetDestination(m_currentDest);

        if (CrossPlatformInputManager.GetButtonDown("Fire1"))
            RipHeart();
    }

    private void RipHeart()
    {
        GameObject plr = GameObject.FindGameObjectWithTag("Player");
        Vector3 targetDir = (transform.position - plr.transform.position).normalized;
        float halfFov = Mathf.Deg2Rad * fov / 2;
        if (Vector3.Dot(targetDir, transform.forward) > Mathf.Cos(halfFov) && CloseToTarget(plr.transform.position, punchDistance))
        {
            AudioSource.PlayClipAtPoint(DeathSound, transform.position);
            Destroy(gameObject);
        }
    }

    private string SerializeVector(Vector3 vec)
    {
        return vec.x + "," + vec.y + "," + vec.z;
    }
    private Vector3 DeserializeVector(string s)
    {
        string[] components = s.Split(new string[] { "," }, StringSplitOptions.None);
        return new Vector3(float.Parse(components[0]), float.Parse(components[1]), float.Parse(components[2]));
    }
}
