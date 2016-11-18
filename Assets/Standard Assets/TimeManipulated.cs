using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TimeManipulated : MonoBehaviour
{
    class StoredPosition
    {
        public Vector3 position;
        public Quaternion rotation;
        public List<string> robotState;
        public StoredPosition(Transform t, List<string> rs)
        {
            position = t.position;
            rotation = t.rotation;
            robotState = rs;
        }
    }
    private TimeState m_timeState;
    private List<string> m_robotState;
    private LinkedList<StoredPosition> m_movementHistory;
    // Use this for initialization
    void Start()
    {
        m_movementHistory = new LinkedList<StoredPosition>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    void FixedUpdate()
    {
        if (m_timeState == TimeState.Normal)
        {
            m_movementHistory.AddFirst(new StoredPosition(transform, m_robotState));
            if (m_movementHistory.Count >= 60 * 10)
                m_movementHistory.RemoveLast();
            GetComponent<RobotMovement>().Move();
        }
        else if (m_timeState == TimeState.Backward)
        {
            if (m_movementHistory.Count > 0)
            {
                StoredPosition sp = m_movementHistory.First.Value;
                transform.position = sp.position;
                transform.rotation = sp.rotation;
                GetComponent<RobotMovement>().SetRobotState(sp.robotState);
                m_movementHistory.RemoveFirst();
            }
        }
    }

    //Set timestate here
    public void SetTimeState(TimeState timeState)
    {
        GetComponent<RobotMovement>().TimeStateChange(m_timeState, timeState);
        m_timeState = timeState;
    }

    public TimeState GetTimeState()
    {
        return m_timeState;
    }

    public void SetRobotState(List<string> robotState)
    {
        m_robotState = robotState;
    }
}