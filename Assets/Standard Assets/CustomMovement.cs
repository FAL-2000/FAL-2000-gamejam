using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class CustomMovement : RobotMovement {
    [SerializeField]
    private float m_timer;

    [SerializeField]
    private Vector3 m_direction = new Vector3(0.1f,0,0);

    public override void Move()
    {
        m_timer += Time.deltaTime;
        if (m_timer >= 3)
        {
            m_direction = -m_direction;
            m_timer = 0;
        }
        transform.position += m_direction;
        List<string> state = new List<string>();
        state.Add(m_timer.ToString());
        state.Add(m_direction.x + "," + m_direction.y + "," + m_direction.z);
        GetComponent<TimeManipulated>().SetRobotState(state);
    }

    public override void SetRobotState(List<string> robotState)
    {
        m_timer = float.Parse(robotState[0]);
        string[] components = robotState[1].Split(new string[] { "," }, StringSplitOptions.None);
        m_direction = new Vector3(float.Parse(components[0]), float.Parse(components[1]), float.Parse(components[1]));
    }

    // Use this for initialization
    void Start () {
        m_timer = 0;
	}
	
	// Update is called once per frame
	void Update () {
        Debug.Log(m_timer);
    }
}
