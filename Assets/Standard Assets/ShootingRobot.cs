using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using UnityStandardAssets.CrossPlatformInput;
using System.Linq;

public class ShootingRobot : RobotMovement {
    public float rateOfFire;
    public GameObject bullet;
    public enum AIState { Patrol, Wait, Attack, None };
    public GameObject[] patrolPath;
    public float fov;
    public float patrolWaitTime;
    public float lookAroundTime;
    public float heartRipDistance;
    public float lookAroundAngle;
    public AudioClip alert;

    private Vector3 m_lastVelocity = Vector3.zero;
    private AIState m_aiState = AIState.Patrol;
    private Vector3 m_currentDest;
    private int m_currentPatrolPoint;
    private float m_waitTime;
    private float m_lookAround;
    private float m_timeToFire;
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
                    m_currentDest = transform.position;
                    transform.forward = plr.transform.position - transform.position;
                    m_timeToFire -= Time.fixedDeltaTime;
                    if (m_timeToFire <= 0)
                    {
                        Shoot(plr.transform.position);
                        m_timeToFire = 1 / rateOfFire;
                    }
                }
                if (!CanSee(plr))
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
        state.Add(m_timeToFire.ToString());
        GetComponent<TimeManipulated>().SetRobotState(state);
    }

    private void Shoot(Vector3 target)
    {
        Vector3 origin = transform.position + transform.forward * 0.7f + transform.right * 0.25f + transform.up * 0.68f;
        float corrected = -Vector3.Angle(transform.forward, (target - origin).normalized) / 2;
        Instantiate(bullet, origin, transform.rotation * Quaternion.AngleAxis(corrected, Vector3.up));
    }

    public override void SetRobotState(List<string> robotState)
    {
        m_aiState = (AIState)Enum.Parse(typeof(AIState), robotState[0]);
        m_currentDest = DeserializeVector(robotState[1]);
        m_currentPatrolPoint = int.Parse(robotState[2]);
        m_waitTime = float.Parse(robotState[3]);
        m_lookAround = float.Parse(robotState[4]);
        m_timeToFire = float.Parse(robotState[5]);
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
    void Start()
    {
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
    void Update()
    {
        GetComponent<NavMeshAgent>().SetDestination(m_currentDest);

        if (CrossPlatformInputManager.GetButtonDown("Fire1"))
            RipHeart();
    }

    private void RipHeart()
    {
        GameObject plr = GameObject.FindGameObjectWithTag("Player");
        Vector3 targetDir = (transform.position - plr.transform.position).normalized;
        float halfFov = Mathf.Deg2Rad * fov / 2;
        if (Vector3.Dot (targetDir, transform.forward) > Mathf.Cos (halfFov) && CloseToTarget (plr.transform.position, heartRipDistance)) {
			Debug.Log ("EAT YOUR HEART OUT");
			Destroy(this.gameObject);	//DIEE
		}
    }

    private bool CloseToTarget(Vector3 target, float tol)
    {
        return (transform.position - target).sqrMagnitude < tol * tol;
    }

    private bool CanSee(GameObject target)
    {
        RaycastHit[] hits = Physics.RaycastAll(transform.position, target.transform.position - transform.position).OrderBy(h => h.distance).ToArray();
        GameObject hit = null;
        foreach (RaycastHit h in hits)
        {
            if (h.transform.tag == "Bullet")
                continue;
            hit = h.transform.gameObject;
            break;
        }
        if (hit == null)
            return false;

        Vector3 targetDir = (target.transform.position - transform.position).normalized;
        float halfFov = Mathf.Deg2Rad * fov / 2;
        return Vector3.Dot(targetDir, transform.forward) > Mathf.Cos(halfFov) && hit == target;
    }

    private GameObject NextPatrolPoint()
    {
        return patrolPath[m_currentPatrolPoint];
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
